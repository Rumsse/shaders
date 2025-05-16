using UnityEngine;

public class RopeBuilder : MonoBehaviour
{
    public GameObject segmentPrefab;
    public int segmentCount = 15;
    public Transform startPoint;

    private GameObject[] segments;

    bool debugEnabled = false;
    public Collider collider;
    public Rigidbody rb;

    void Start()
    {
        BuildRope();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            debugEnabled = !debugEnabled;
    }

    void BuildRope()
    {
        segments = new GameObject[segmentCount];
        Vector3 segmentPosition = startPoint.position;

        GameObject previousSegment = null;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, segmentPosition, Quaternion.identity);
            segment.name = "RopeSegment_" + i;

            if (previousSegment != null)
            {
                var joint = segment.GetComponent<ConfigurableJoint>();
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            }
            else
            {
                var joint = segment.GetComponent<ConfigurableJoint>();
                joint.connectedBody = startPoint.GetComponent<Rigidbody>();
            }

            previousSegment = segment;
            segmentPosition.y -= 0.2f; // odstêp miêdzy segmentami
            segments[i] = segment;
        }
    }

    void OnDrawGizmos()
    {
        if (!debugEnabled) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collider.bounds.size);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
    }

    void OnGUI()
    {
        if (debugEnabled)
        {
            GUI.Label(new Rect(10, 10, 300, 20), "Velocity: " + rb.linearVelocity.ToString());
            GUI.Label(new Rect(10, 30, 300, 20), "Mass: " + rb.mass);
        }
    }


}
