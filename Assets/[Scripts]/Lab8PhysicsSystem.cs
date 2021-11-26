using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class Lab8PhysicsSystem : MonoBehaviour
{
    public Slider gravityScaleSlider;
    public Toggle gravityCheckBox;
    public List<Lab8PhysicsObjects> lab8Physics = new List<Lab8PhysicsObjects>();
    private Vector3 gravity = new Vector3(0, 0, 0);
    public List<PhysicsCollider> ColliderShapes;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float gravityValue = gravityScaleSlider.value;
        gravity = new Vector3(0, gravityValue, 0);

        if (gravityCheckBox.isOn)
        {
            for (int i = 0; i < lab8Physics.Count; i++)
            {
                if (lab8Physics[i].shape.GetCollistionShape() == CollistionShape.Sphere)
                {
                    lab8Physics[i].velocity += gravity * Time.fixedDeltaTime;
                }

            }
        }
        else
        {
            gravity = new Vector3(0, 0, 0);
        }

        CollisionUpdate();
    }
    void CollisionUpdate()
    {
        for (int i = 0; i < lab8Physics.Count; i++)
        {
            for (int j = i + 1; j < lab8Physics.Count; j++)
            {
                Lab8PhysicsObjects ObjectA = lab8Physics[i];
                Lab8PhysicsObjects ObjectB = lab8Physics[j];

                Vector3 ObjectAPosition = ObjectA.transform.position;
                Vector3 ObjectBPosition = ObjectB.transform.position;
                Vector3 displacement = ObjectBPosition - ObjectAPosition;

                if (ObjectA.shape == null || ObjectB.shape == null)
                {
                    continue;
                }

                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Sphere
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Sphere)
                {
                    //float distance = Mathf.Sqrt(Mathf.Pow(ObjectAPosition.x - ObjectBPosition.x, 2) +
                    //                            Mathf.Pow(ObjectAPosition.y - ObjectBPosition.y, 2) +
                    //                            Mathf.Pow(ObjectAPosition.z - ObjectBPosition.z, 2));
                    //float penetrationdepth = (((PhysicsColliderSphere)ColliderShapes[i]).getRaduis() +
                    //                                        ((PhysicsColliderSphere)ColliderShapes[j]).getRaduis()) - (distance);
                    //if ((distance) <= (((PhysicsColliderSphere)ColliderShapes[i]).getRaduis() + ((PhysicsColliderSphere)ColliderShapes[j]).getRaduis()))
                    //{
                    //    ObjectA.velocity = Vector3.zero;
                    //    ObjectB.velocity = Vector3.zero;
                    //    //Debug.Log(ObjectA.name + " and " + ObjectB.name + " collided");
                    //}
                    //else
                    //{
                    //    return;
                    //}

                    //Vector3 CollisionNormalAToB = displacement/distance;
                    //Vector3 MinimumTranslationVectorA = -penetrationdepth * CollisionNormalAToB * 0.5f;
                    //Vector3 MinimumTranslationVectorB =  penetrationdepth * CollisionNormalAToB * 0.5f;

                    //ObjectA.transform.position += MinimumTranslationVectorA;
                    //ObjectB.transform.position += MinimumTranslationVectorB;
                    SphereSphereCollision((PhysicsColliderSphere)ObjectA.shape, (PhysicsColliderSphere)ObjectB.shape);

                }

                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Sphere
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Plane)
                {
                    SpherePlaneCollision((PhysicsColliderSphere) ObjectA.shape, (PhysicsColliderPlane) ObjectB.shape);
                }


                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Plane
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Sphere)
                {
                    SpherePlaneCollision((PhysicsColliderSphere) ObjectB.shape, (PhysicsColliderPlane) ObjectA.shape);
                }

            }
        }
    }

    static void SphereSphereCollision(PhysicsColliderSphere sphere1, PhysicsColliderSphere sphere2)
    {
        Vector3 Displacement = sphere2.transform.position - sphere1.transform.position;
        float Distance = Displacement.magnitude;
        float SumRaduis = sphere1.getRaduis() + sphere2.getRaduis();
        float PenetrationDepth = SumRaduis - Distance;
        bool IsOverLapping = PenetrationDepth > 0;
        if (IsOverLapping)
        {
        }
        else
        {
            return;
        }

        float minimumDistance = 0.0001f;
        if (Distance < minimumDistance)
        {
            Distance = minimumDistance;
        }
        Vector3 CollisionNormalAToB = Displacement / Distance;
        Vector3 MinimumTranslationVector1 = -PenetrationDepth * CollisionNormalAToB * 0.5f;
        Vector3 MinimumTranslationVector2 = PenetrationDepth * CollisionNormalAToB * 0.5f;

        sphere1.transform.position += MinimumTranslationVector1;
        sphere2.transform.position += MinimumTranslationVector2;
    }
    static void SpherePlaneCollision(PhysicsColliderSphere sphere, PhysicsColliderPlane plane)
    {
        Vector3 PointonOnPlane = plane.transform.position;
        Vector3 CenterOfSphere = sphere.transform.position;
        Vector3 FromPlaneToSphere = CenterOfSphere - PointonOnPlane;
        float dot = Vector3.Dot(FromPlaneToSphere, plane.getNormal());
        Vector3 Displacement = PointonOnPlane - sphere.transform.position;
        float Distance = dot;
        float penetrationdepth = sphere.getRaduis() - Distance;
        bool isOverLapping = penetrationdepth > 0;
        if (isOverLapping)
        {
            sphere.KinematicsObject.velocity = Vector3.zero;
        }
        else
        {
            return;
        }

        //float minimumDistance = 0.0001f;
        //if (Distance < minimumDistance)
        //{
        //    Distance = minimumDistance;
        //}
        //Vector3 CollisionNormalAToB = Displacement / Distance;
        //Vector3 MinimumTranslationVector = -penetrationdepth * CollisionNormalAToB * 0.5f;

        //sphere.transform.position += MinimumTranslationVector;


    }


}
