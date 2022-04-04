using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//todo move collision sounds to collision handler
public class CollisionHandler : MonoBehaviour
{   
    [SerializeField] 
    private float m_sceneLoadDelay = 2f;
    [SerializeField] [Tooltip("success, explosion")]
    List<ParticleSystem> m_rocketParticles = new List<ParticleSystem>();
    
    private bool m_isSceneTransitioning = false;
    private bool m_processCollision = false;

#region MidLevelMethods
    private void OnRocketCrashed()
    {
        // if(GetComponent<Movement>().IsSceneLoading())
        //     return;

        GetComponent<Movement>().enabled = false;
        GetComponent<Movement>().OnDeath();
        m_rocketParticles[1].Play();
        // GameObject.Find("Explosion Particles").GetComponent<ParticleSystem>().Play();

        m_isSceneTransitioning = true;
        Invoke("ReloadScene", m_sceneLoadDelay);
    }

    private void OnLandPadCollision()
    {
        // if(GetComponent<Movement>().IsSceneLoading())
        //     return;
            
        GetComponent<Movement>().enabled = false;
        GetComponent<Movement>().OnSceneFinish();
        m_rocketParticles[0].Play();

        m_isSceneTransitioning = true;
        Invoke("LoadNextScene", m_sceneLoadDelay);
    }

    private void ReloadScene()
    {
        string activeScenePath = SceneManager.GetActiveScene().path;
        SceneManager.LoadScene(activeScenePath);

        GetComponent<Movement>().enabled = true;
    }

    private void LoadNextScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        int nextSceneIndex = activeScene.buildIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
        GetComponent<Movement>().enabled = true;
    }

    private void ProcessDebugInput()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null && keyboard.lKey.wasPressedThisFrame)
        {
            LoadNextScene();
        }
        else if (keyboard != null && keyboard.cKey.wasPressedThisFrame)
        {
            m_processCollision = !m_processCollision;
        }
#endif
    }
#endregion

    private void OnCollisionEnter(Collision other) 
    {
        if(m_isSceneTransitioning || m_processCollision)
            return;

        switch(other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                OnLandPadCollision();
                break;
            default:
                OnRocketCrashed();
                break;
        }    
    }

    private void Update()
    {
        ProcessDebugInput();
    }
}
