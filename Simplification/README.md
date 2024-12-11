# Simplification, Subdivision and Interpolation algorithms

You can find 4 prefabs in the prefab folder, each one implementing a different method.
The main scene is an example scene with all prefabs placed with nice parameters, the simplification algorithm can be seen only at runtime while the others are visible on gizmos.

## Simplification : Cell Collapse

The ObfSimplification prefab takes an obf file and simplifies the mesh using the Cell Collapse method. I use Octrees to subdivide space and create Representative points, you can check the *Show representatives* parameter to see them. Depth is the depth of the octree (The less deth the less details)

## Subdivision : Chaikin

The CurveSubdivision prefab Takes a variable amount of points and a number of subdivisions to create a Polygon and subdivide it using Chaikin's algorithm.

## Interpolation : Hermite and Bezier

The other two prefabs create curves with a variable precision either by using Hermite's cubic or Bezier's curve. Everything is visible on the Gizmos and parameters can be changed in the editor since they are *Serialized*
