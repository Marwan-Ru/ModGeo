using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * The octree class has been modified to contain vertices
 * in leaves
 */

public class Octree
{
    bool _isLeaf = false;
    bool _hasRepresentative = false;
    private List<Octree> _childrens;
    (Vector3, Vector3) _boundingBox;
    private List<int> _verticesIndices = new List<int>();
    private int _representativeIndice;

    public Octree((Vector3, Vector3) octreeRect, List<int> vertices)
    {
        _boundingBox = octreeRect;
        _isLeaf = true;
        _verticesIndices = vertices;
    }

    public Octree(List<Octree> childrens, (Vector3, Vector3) octreeRect)
    {
        _childrens = childrens;
    }

    public bool IsLeaf()
    {
        return _isLeaf;
    }
    
    public bool HasRepresentative()
    {
        return _hasRepresentative;
    }
    
    public (Vector3, Vector3) GetBoundingBox()
    {
        return _boundingBox;
    }

    public Octree GetChild(int index)
    {
        return _childrens[index];
    }

    public List<int> GetVerticesIndices()
    {
        return _verticesIndices;
    }

    public void SetRepresentative(int i)
    {
        _hasRepresentative = true;
        _representativeIndice = i;
    }

    public int GetRepresentativeIndice()
    {
        return _representativeIndice;
    }
}