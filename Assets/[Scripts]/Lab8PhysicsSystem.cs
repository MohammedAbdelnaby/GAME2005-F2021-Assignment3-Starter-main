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

                if (ObjectA.shape == null || ObjectB.shape == null)
                {
                    continue;
                }

                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Sphere
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Sphere)
                {
                    float distance = Mathf.Sqrt(Mathf.Pow(ObjectAPosition.x - ObjectBPosition.x, 2) +
                                                Mathf.Pow(ObjectAPosition.y - ObjectBPosition.y, 2) +
                                                Mathf.Pow(ObjectAPosition.z - ObjectBPosition.z, 2));
                    float penetrationdepth = (((PhysicsColliderSphere)ColliderShapes[i]).getRaduis() +
                                                            ((PhysicsColliderSphere)ColliderShapes[j]).getRaduis()) - (distance);
                    if ((distance) <= (((PhysicsColliderSphere)ColliderShapes[i]).getRaduis() + ((PhysicsColliderSphere)ColliderShapes[j]).getRaduis()))
                    {
                        ObjectA.velocity = Vector3.zero;
                        ObjectB.velocity = Vector3.zero;
                        //Debug.Log(ObjectA.name + " and " + ObjectB.name + " collided");
                    }
                }

                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Sphere
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Plane)
                {
                    if (SpherePlaneCollision((PhysicsColliderSphere)ObjectA.shape, (PhysicsColliderPlane)ObjectB.shape))
                    {
                        //Debug.Log(ObjectA.name + " and " + ObjectB.name + " collided");
                        Color colorSphere = ObjectA.GetComponent<Renderer>().material.color;
                        Color colorPlane = ObjectB.GetComponent<Renderer>().material.color;
                        ObjectA.GetComponent<Renderer>().material.color = Color.Lerp(colorSphere, colorPlane, 0.05f);
                        ObjectB.GetComponent<Renderer>().material.color = Color.Lerp(colorPlane, colorSphere, 0.05f);
                        ObjectA.velocity = Vector3.zero;
                    }


                }


                if (ObjectA.shape.GetCollistionShape() == CollistionShape.Plane
                    && ObjectB.shape.GetCollistionShape() == CollistionShape.Sphere)
                {
                    if (SpherePlaneCollision((PhysicsColliderSphere)ObjectB.shape, (PhysicsColliderPlane)ObjectA.shape))
                    {
                        //Debug.Log(ObjectB.name + " and " + ObjectA.name + " collided");
                        Color colorSphere = ObjectB.GetComponent<Renderer>().material.color;
                        Color colorPlane = ObjectA.GetComponent<Renderer>().material.color;
                        ObjectB.GetComponent<Renderer>().material.color = Color.Lerp(colorPlane, colorSphere, 0.05f);
                        ObjectA.GetComponent<Renderer>().material.color = Color.Lerp(colorSphere, colorPlane, 0.05f);
                        ObjectB.velocity = Vector3.zero;
                    }

                }

            }
        }
    }

    static bool SpherePlaneCollision(PhysicsColliderSphere sphere, PhysicsColliderPlane plane)
    {
        Vector3 PointonOnPlane = plane.transform.position;
        Vector3 CenterOfSphere = sphere.transform.position;
        Vector3 FromPlaneToSphere = CenterOfSphere - PointonOnPlane;
        float dot = Vector3.Dot(FromPlaneToSphere, plane.getNormal());
        float Distance = Mathf.Abs(dot);
        float penetrationdepth = sphere.getRaduis() - Distance;
        bool isOverLapping = penetrationdepth > 0;
        return isOverLapping;
    }
}
