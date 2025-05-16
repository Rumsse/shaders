using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position += new Vector3(0, 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.position += new Vector3(0, -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.position += new Vector3(1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position += new Vector3(0, 0, 1);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position += new Vector3(-1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            transform.position += new Vector3(0, 0, -1);
        }
    }
}
