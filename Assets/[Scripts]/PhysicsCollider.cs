using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollistionShape
{
    Sphere,
    Plane,
    AABB
}

public abstract class PhysicsCollider : MonoBehaviour
{

    public abstract CollistionShape GetCollistionShape();

    public void Start()
    {
        FindObjectOfType<Lab8PhysicsSystem>().ColliderShapes.Add(this);
    }
}