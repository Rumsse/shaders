using UnityEngine;

public class Explorer : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] Vector2 pos;
    [SerializeField] float scale;
    [SerializeField] float angle;

    private Vector2 smoothPos;
    private float smoothScale;
    private float smoothAngle;


    private void FixedUpdate()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

        UpdateShader();
        HandleInput();
    }

    private void UpdateShader()
    {
        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspect > 1f)
        {
            scaleY /= aspect;
        }
        else
        {
            scaleX *= aspect;
        }

        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            scale *= 0.99f;
        }
        if (Input.GetKey(KeyCode.X))
        {
            scale *= 1.01f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            angle += 0.01f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            angle -= 0.01f;
        }

        Vector2 dir = new Vector2(0.01f * scale, 0);
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        dir = new Vector2(dir.x * cos, dir.x * sin);


        
        if (Input.GetKey(KeyCode.A))
        {
            pos -= dir;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += dir;
        }
        dir = new Vector2(-dir.y, dir.x);
        if (Input.GetKey(KeyCode.W))
        {
            pos += dir;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos -= dir;
        }

    }

}
