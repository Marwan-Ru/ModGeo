using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Octree
{
    bool _isLeaf = false;
    bool _isInside = false;
    private List<Octree> _childrens;
    (Vector3, Vector3) _boundingBox;

    public Octree(bool isInside, (Vector3, Vector3) octreeRect)
    {
        _boundingBox = octreeRect;
        _isLeaf = true;
        _isInside = isInside;
    }

    public Octree(List<Octree> childrens, (Vector3, Vector3) octreeRect)
    {
        _childrens = childrens;
    }

    public bool IsLeaf()
    {
        return _isLeaf;
    }

    public bool IsInside()
    {
        return _isInside;
    }

    public (Vector3, Vector3) GetBoundingBox()
    {
        return _boundingBox;
    }

    public Octree GetChild(int index)
    {
        return _childrens[index];
    }
}