using UnityEngine;
using System.Collections.Generic;

public class GravityZone : MonoBehaviour
{
    public Vector3 gravityDirection = Vector3.down;
    public float gravityStrength = 9.81f;

    private List<Rigidbody> bodiesInZone = new List<Rigidbody>();

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb && !bodiesInZone.Contains(rb))
        {
            rb.useGravity = false;
            bodiesInZone.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb && bodiesInZone.Contains(rb))
        {
            rb.useGravity = true;
            bodiesInZone.Remove(rb);
        }
    }

    void FixedUpdate()
    {
        foreach (Rigidbody rb in bodiesInZone)
        {
            if (rb != null)
            {
                rb.AddForce(gravityDirection.normalized * gravityStrength, ForceMode.Acceleration);
            }
        }
    }
}
