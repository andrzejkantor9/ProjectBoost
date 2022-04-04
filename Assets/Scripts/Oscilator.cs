using UnityEngine;

public class Oscilator : MonoBehaviour
{
    private Vector3 m_startingPosition = Vector3.zero;
    [SerializeField]
    private Vector3 m_movementVector = Vector3.zero;
    // [SerializeField] [Range(0f, 1f)]
    // private float  m_movementFactor;
    [SerializeField]
    private float period = 2f;

    //~ 6.283
    private const float TAU = Mathf.PI * 2;

    private void Start()
    {
        m_startingPosition = transform.position;        
    }

    private void Update()
    {
        ProcessOscilatingMovement();
    }

    private void ProcessOscilatingMovement()
    {
        //continually growin over time
        if(period <= Mathf.Epsilon)
            return;

        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * TAU);

        //0-1f
        float m_movementFactor = rawSinWave + 1f / 2f;

        Vector3 offset = m_movementVector * m_movementFactor;
        transform.position = m_startingPosition + offset;
    }
}
