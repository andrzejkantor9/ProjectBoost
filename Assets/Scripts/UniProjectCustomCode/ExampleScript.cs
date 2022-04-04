using UnityEngine;

//todo's
public class ExampleScript : MonoBehaviour
{
    //CACHE
    // [SerializeField][HideInInspector]
    // private BoxCollider m_boxCollider = null;
    //PROPERTIES
    [SerializeField]
    private float m_speed = 1f; 
    //[SerializeField] [Range(0,1)] [Tooltip("to display in inspector")]
    //STATES
    private bool m_isDead = false;
    const string FRIENDLY_TAG = "Friendly";

    ///////////////////////////////////////////////
    //only engine methods without regions
    //only methods inside engine methods
    //#if DEVELOPMENT_BUILD || UNITY_EDITOR

    private void OnValidate()
    {
        SetupComponents();
        AssertComponents();
    }


    private void Awake() 
    {
        AssertComponents(); 

        //only for naming convention, use methods instead of code inside
        bool awakeFinished;       
    }

    private void Update() 
    {
        UnityEngine.Profiling.Profiler.BeginSample($"{GetType()}: Update");

        //update code here

        UnityEngine.Profiling.Profiler.EndSample();
    }

#region MidLevelCode

    private void SetupComponents()
    {
        // m_boxCollider = GetComponent<BoxCollider>();
    }

    private void AssertComponents()
    {
        // UnityEngine.Assertions.Assert.IsNotNull(m_boxCollider, $"Script: {GetType().ToString()} variable m_boxCollider is null");
    }

#endregion

#region LowLevelCode
#endregion

}