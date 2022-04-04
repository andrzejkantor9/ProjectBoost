using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

//todo - search all not loaded scenes (all in project)
//todo - or on validate save
//todo - search objects recursively
//todo - list unsued tags as well
//todo - create enum variable

//EXTRA FEATURES
//todo - search one by one scene, or all at once
//todo - toggle debug for not found game objects
//todo - make it work when new tag is added, not on menu item call

public class GenerateTagsInCode : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("CustomTools/GenerateTagsInCode")]
    private static void GenerateTags()
    {
        //clear console
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);

        var allAssetsPaths = UnityEditor.AssetDatabase.GetAllAssetPaths();
        HashSet<string> hashset = new HashSet<string>();

        foreach (var assetPath in allAssetsPaths)
        {
            if(!assetPath.StartsWith("Assets"))
                continue;
            var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if(go)
            {
                Debug.Log($"GameObject: {go.ToString()}, its tag to add: {go.tag}, its path: {assetPath}");
                hashset.Add(go.tag);
            }    
            else
            {
                // Debug.Log($"could not find gameobject asset at path: {assetPath}");
            }     

            // List<Scene> allScenes = new List<Scene>();

            // for (int i = 0; i < SceneManager.; i++)
            // {
            //     allScenes.Add(SceneManager.GetSceneAt(i));
            // } 

            // string currentSceneName = SceneManager.GetActiveScene().path;
            // for (int i = 0; i < SceneManager.sceneCount; i++)
            // {
            //     SceneManager.LoadScene()
            // }

            // SceneManager.LoadScene(currentScene.name);
        }

        foreach (var item in hashset)
        {
            Debug.Log($"tag: {item}");
        }
    }
    #endif
}
