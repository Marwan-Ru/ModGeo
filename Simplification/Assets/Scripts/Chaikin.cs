using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;
// ReSharper disable SuggestVarOrType_BuiltInTypes

public class Chaikin : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private int subdivisions = 0;

    [SerializeField] private List<Vector3> bounds;

    private List<Vector3> _points;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < bounds.Count; i++)
        {
            int i2 = i < bounds.Count - 1 ? i + 1 : 0;
            Gizmos.DrawLine(transform.position + bounds[i], transform.position + bounds[i2]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Subdivide();
        
        for (int i = 0; i < _points.Count; i++)
        {
            int i2 = i < _points.Count - 1 ? i + 1 : 0;
            Gizmos.DrawLine(transform.position + _points[i], transform.position + _points[i2]);
        }
    }

    private void Subdivide()
    {
        _points = bounds;
        
        for (int i = 0; i < subdivisions; i++)
        {
            var tmp = new List<Vector3>();
            for (int j = 0; j < _points.Count; j++)
            {  
                int j2 = j < _points.Count - 1 ? j + 1 : 0;
                
                tmp.Add(0.75f * _points[j] + (_points[j2] * 0.25f));
                tmp.Add(0.25f * _points[j] + (_points[j2] * 0.75f));
            }
            _points = tmp;
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        
    }
}
