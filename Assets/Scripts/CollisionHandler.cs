using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Movement movement = GetComponent<Movement>();
        if(!movement)
            return;

        movement.enabled = false;
        movement.OnDeath();
        m_rocketParticles[1].Play();

        m_isSceneTransitioning = true;
        //invokes are ugly, just for presentation
        Invoke("ReloadScene", m_sceneLoadDelay);
    }

    private void OnLandPadCollision()
    {            
        Movement movement = GetComponent<Movement>();
        if(!movement)
            return;

        movement.enabled = false;
        movement.OnSceneFinish();

        m_rocketParticles[0].Play();

        m_isSceneTransitioning = true;
        //invokes are ugly, just for presentation
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
