using UnityEngine;

public class Movement : MonoBehaviour
{
    private UnityEngine.InputSystem.Keyboard m_keyboard = null;
    
    [SerializeField] private Rigidbody m_rigidbody = null;
    [SerializeField] private AudioSource m_audioSource = null;
    
    [SerializeField] private float m_ThrustPower = 1000f;
    [SerializeField] private float m_RotationSpeed = 100f;

#region debugVariables
    private float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();
#endregion

    private void Awake() 
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_rigidbody, $"{this.GetType().ToString()} script, m_rigidbody = null!");
        UnityEngine.Assertions.Assert.IsNotNull(m_audioSource, $"{this.GetType().ToString()} script, m_audioSource = null!");
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }

    private void Start()
    {
        SetupFpsCounter();
    }

    void Update()
    {
        m_keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (m_keyboard != null)
        {
            ProcessThrust();
            ProcessRotation();

            ProcessMaximizeEditorPlayWindow();
        }

        ProcessFpsCounter();
    }

    private void ProcessThrust()
    {
        if (m_keyboard.spaceKey.isPressed)
        {
            m_rigidbody.AddRelativeForce(Vector3.up * m_ThrustPower * Time.deltaTime);
            if(!m_audioSource.isPlaying)
                m_audioSource.Play();
        }            
        else
        {
            if(m_audioSource.isPlaying)
                m_audioSource.Stop();
        }
    }

    private void OnDisable() 
    {
        if(m_audioSource.isPlaying)
            m_audioSource.Stop();
    }

    private void ProcessRotation()
    {
        if (m_keyboard.aKey.isPressed)
            ApplyRotation(true);
        else if (m_keyboard.dKey.isPressed)
            ApplyRotation(false);
    }

    private void ApplyRotation(bool rotateLeft)
    {
        //freezing rotation so we can manually rotate
        m_rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * m_RotationSpeed * Time.deltaTime * (rotateLeft ? -1f : 1f));

        m_rigidbody.freezeRotation = false;
    }

    #region debug

    private void ProcessMaximizeEditorPlayWindow()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying && m_keyboard.f11Key.wasPressedThisFrame)
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
#endif
    }

    private void ProcessFpsCounter()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    private void SetupFpsCounter()
    {
        timeleft = updateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + "FPS", textStyle);

        
    }
#endregion
}
