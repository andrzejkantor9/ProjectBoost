using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{   
    [SerializeField] private float SceneLoadDelay = 1f;

    private void OnCollisionEnter(Collision other) 
    {
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

    //todo add sfx upon crash, and success
    //todo add vfx upon crash, and success
    private void OnRocketCrashed()
    {
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadScene", SceneLoadDelay);
    }

    private void OnLandPadCollision()
    {
        GetComponent<Movement>().enabled = false;

        Invoke("LoadNextScene", SceneLoadDelay);
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
}
