using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private static float gravityForce = 800;

    public Vector3 GravityDirection
    {
        get
        {
            if (gravityAreas.Count == 0) return Vector3.zero;
            gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody rigidbody;
    private List<GravityArea> gravityAreas;

    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        gravityAreas = new List<GravityArea>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(GravityDirection * (gravityForce * Time.fixedDeltaTime), ForceMode.Acceleration);

        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
        Quaternion newRotation = Quaternion.Slerp(rigidbody.rotation, upRotation * rigidbody.rotation, Time.fixedDeltaTime * 3f); ;
        rigidbody.MoveRotation(newRotation);
    }

    public void AddGravityArea(GravityArea gravityArea)
    {
        gravityAreas.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        gravityAreas.Remove(gravityArea);
    }
}