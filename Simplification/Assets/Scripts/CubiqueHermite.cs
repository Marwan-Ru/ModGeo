using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable SuggestVarOrType_BuiltInTypes

public class CubiqueHermite : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 startDirection;
    
    
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 endDirection;

    [Range(2, 100)]
    [SerializeField] private int discreetDivisions;

    private List<Vector3> _points;

    private void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        // Drawing start and end position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + startPosition, 0.4f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + endPosition, 0.4f);

        // Drawing start and end direction
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position + startPosition - startDirection, transform.position + startPosition + startDirection);
        Gizmos.DrawLine(transform.position + endPosition - endDirection, transform.position + endPosition + endDirection);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Hermite();
        
        for (int i = 0; i < _points.Count - 1; i++)
        {
            Gizmos.DrawLine(transform.position + _points[i], transform.position + _points[i + 1]);
        }
    }
    
    private void Hermite()
    {
        float distance = Vector3.Distance(endPosition, startPosition);
        _points = new List<Vector3>();
        float div = (float) discreetDivisions;
        
        for (int i = 0; i < discreetDivisions; i++)
        {
            float u = (1.0f / div) * i;

            float f1 = 2 * u * u * u - 3 * u * u + 1;
            float f2 = -2 * u * u * u + 3 * u * u;
            float f3 = u * u * u - 2 * u * u + u;
            float f4 = u * u * u - u * u;
            
            _points.Add(startPosition * f1 + endPosition * f2 + startDirection * f3 + endDirection * f4);
        }
    }
}
