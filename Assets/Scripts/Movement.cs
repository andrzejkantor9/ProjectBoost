using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //CACHE
    [SerializeField][HideInInspector]
    private Rigidbody m_rigidbody = null;
    [SerializeField][HideInInspector]
    private AudioSource m_audioSource = null;
    
    [SerializeField] [Tooltip("engine, success, death clips")]
    private List<AudioClip> m_rocketAudioClips = new List<AudioClip>();
    [SerializeField] [Range(0f, 1f)]
    private float m_deathClipVolume = 1f;

    [SerializeField] [Tooltip("main thruster, left, right thrusters")]
    List<ParticleSystem> m_thrusterParticles = new List<ParticleSystem>();
    
    //PARAMETERS
    [SerializeField] 
    private float m_ThrustPower = 1000f;
    [SerializeField] 
    private float m_RotationSpeed = 100f;

    //STATE
    private UnityEngine.InputSystem.Keyboard m_keyboard = null;

#region LowLevelMethods
    private void AssertComponents()
    {
        UnityEngine.Assertions.Assert.IsNotNull(m_rigidbody, $"{this.GetType().ToString()} script, m_rigidbody = null!");
        UnityEngine.Assertions.Assert.IsNotNull(m_audioSource, $"{this.GetType().ToString()} script, m_audioSource = null!");
    }

    private void GetComponentOnValidate()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();

// #if UNITY_EDITOR
//         UnityEditor.EditorUtility.SetDirty(this.gameObject);
// #endif
    }

        private static void UnlimitFramerate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
    }

    private void OnStopThrusting()
    {
        m_audioSource.Stop();
        m_thrusterParticles[0].Stop();
    }

    private void OnStartThrusting()
    {
        m_rigidbody.AddRelativeForce(Vector3.up * m_ThrustPower * Time.deltaTime);
        if (!m_audioSource.isPlaying)
            m_audioSource.PlayOneShot(m_rocketAudioClips[0]);

        if (!m_thrusterParticles[0].isPlaying)
            m_thrusterParticles[0].Play();
    }

    private void ApplyRotation(bool rotateLeft)
    {
        //freezing rotation so we can manually rotate
        m_rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * m_RotationSpeed * Time.deltaTime * (rotateLeft ? -1f : 1f));

        m_rigidbody.freezeRotation = false;
    }
    
    private void OnStopRotation()
    {
        m_thrusterParticles[1].Stop();
        m_thrusterParticles[2].Stop();
    }

    private void OnRotateRight()
    {
        ApplyRotation(false);

        if (!m_thrusterParticles[1].isPlaying)
            m_thrusterParticles[1].Play();
    }

    private void OnRotateLeft()
    {
        ApplyRotation(true);

        if (!m_thrusterParticles[2].isPlaying)
            m_thrusterParticles[2].Play();
    }
#endregion

#region MidLevelMethods
    private void ProcessThrust()
    {
        if (m_keyboard.spaceKey.isPressed)
        {
            OnStartThrusting();
        }
        else
        {
            OnStopThrusting();
        }
    }

    private void ProcessRotation()
    {
        if (m_keyboard.aKey.isPressed)
        {
            OnRotateLeft();
        }
        else if (m_keyboard.dKey.isPressed)
        {
            OnRotateRight();
        }
        else
        {
            OnStopRotation();
        }

    }

    public void OnSceneFinish()
    {
        m_audioSource.PlayOneShot(m_rocketAudioClips[1]);
    }

    public void OnDeath()
    {
        m_audioSource.PlayOneShot(m_rocketAudioClips[2], m_deathClipVolume);

        foreach (var particleObject in m_thrusterParticles)
        {
            particleObject.Stop();
        }
    }
#endregion

    private void OnValidate()
    {
        GetComponentOnValidate();
        AssertComponents();
    }

    private void Awake()
    {
        UnlimitFramerate();
        AssertComponents();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        m_keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (m_keyboard != null)
        {
            ProcessThrust();
            ProcessRotation();

        }
    }

    private void OnDisable() 
    {
        m_audioSource.Stop();

        foreach (var particleObject in m_thrusterParticles)
        {
            particleObject.Stop();
        }
    }
}
