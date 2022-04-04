using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    private Vector3 m_startingPosition = Vector3.zero;
    [SerializeField]
    private Vector3 m_movementVector = Vector3.zero;
    [SerializeField] [Range(0f, 1f)]
    private float  m_movementFactor = 0f;

    private void Start()
    {
        m_startingPosition = transform.position;        
    }

    private void Update()  
    {
        Vector3 offset = m_movementVector * m_movementFactor;
        transform.position = m_startingPosition + offset;        
    }
}
