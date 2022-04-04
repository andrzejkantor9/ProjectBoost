using UnityEngine;

//todo refactor fps counter code
//todo scale gui according to screen
//todo fix - f11 has to be pressed twice on window minimize
//todo - namescape CustomDebug and separate classes
public class CustomDebug : MonoBehaviour 
{
    private float m_fpsCounterUpdateInterval = 0.5f; 

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    GUIStyle textStyle = new GUIStyle();

    ///////////////////////////////////////////////

    private void Awake() 
    {
        SetupFpsCounter();
    }

    void Update()
    {
        ProcessMaximizeEditorPlayWindow();
        ProcessFpsCounter();        
    }
    
#region MidLevelCode

    public static void Log(string message)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        UnityEngine.Debug.Log(message);
#endif
    }

    private void ProcessMaximizeEditorPlayWindow()
    {
#if UNITY_EDITOR
        UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (UnityEditor.EditorApplication.isPlaying && keyboard.f11Key.wasPressedThisFrame)
            {
                UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
            }
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
            timeleft = m_fpsCounterUpdateInterval;
            accum = 0.0f;
            frames = 0;
        }

        m_lastUpdateTime = Time.time;
    }

    private void SetupFpsCounter()
    {
        timeleft = m_fpsCounterUpdateInterval;

        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    float m_lastUpdateTime = 0f;
    void OnGUI()
    {
        //Display the fps and round to 2 decimals
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + "FPS", textStyle);      
    }   

#endregion
}