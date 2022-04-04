using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    private UnityEngine.InputSystem.Keyboard m_keyboard = null;

    private void Update() 
    {
        m_keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (m_keyboard != null)        
        {
            if(m_keyboard.escapeKey.wasPressedThisFrame)
            {
                Debug.Log("QuitApplication");
                Application.Quit();
            }
            
        }
    }
}
