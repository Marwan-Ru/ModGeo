using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    
    [SerializeField]
    private Vector3[] _cp = { new Vector3(-2f, -2f, 0f), new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 0f), new Vector3(2f, -2f, 0f) };

    private int _currentControlledCP = 0;
    
    [Range(2, 100)]
    [SerializeField] private int discreetDivisions;

    private List<Vector3> _points;
    
    private void OnDrawGizmos()
    {
        // Drawing Control points
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + _cp[_currentControlledCP], 0.2f);
    
        // Drawing polygon with control points
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(transform.position + _cp[0], transform.position + _cp[1]);
        Gizmos.DrawLine(transform.position + _cp[1], transform.position + _cp[2]);
        Gizmos.DrawLine(transform.position + _cp[2], transform.position + _cp[3]);
        Gizmos.DrawLine(transform.position + _cp[3], transform.position + _cp[0]);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        BezierCurve();
        
        for (int i = 0; i < _points.Count - 1; i++)
        {
            Gizmos.DrawLine(transform.position + _points[i], transform.position + _points[i + 1]);
        }
    }

    private void BezierCurve()
    {
        _points = new List<Vector3>();
        float div = (float) discreetDivisions;

        _points.Add( _cp[0]);
        
        for (int i = 0; i < discreetDivisions; i++)
        {
            float t = (1.0f / div) * i;

            var Qt = Vector3.zero;
            
            for (int j = 0; j < 4; j++) // Somme de 0 a n-1
            {
                // Pi * Bi,n-1(t)
                Qt += _cp[j] * (fact(3) / (fact(j) * fact(3.0f - j))) * Mathf.Pow(t, j) * Mathf.Pow(1 - t, 3.0f - j);
            }
            
            _points.Add(Qt);
        }
        
        _points.Add(_cp[3]);
    }

    private float fact(float x)
    {
        float result = 1;

        for (int i = 1; i <= x; i++)
        {
            result *= i;
        }
        
        return result;
    }
}
