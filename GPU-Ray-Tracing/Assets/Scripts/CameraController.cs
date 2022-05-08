using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 10.0f;
    public float cameraRotateSpeed = 1000.0f;

    private Camera _camera;
    private Vector2 rotation;
    private float tiltAngle = 0;

    private Vector2 GetInput() {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        return input;
    }

    private void TranslateTo(Vector3 direction) {
        transform.position += direction * cameraMoveSpeed * Time.deltaTime;
    }
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            TranslateTo(transform.forward);
        }
        if (Input.GetKey("s")) {
            TranslateTo(-transform.forward);
        }
        if (Input.GetKey("a")) {
            TranslateTo(-transform.right);
        }
        if (Input.GetKey("d")) {
            TranslateTo(transform.right);
        }
        if (Input.GetKey("space")) {
            TranslateTo(Vector3.up);
        }
        if (Input.GetKey("left shift")) {
            TranslateTo(Vector3.down);
        }

        

        if (Input.GetMouseButton(0)) {
            Vector2 curAngularVelocity = GetInput() * cameraRotateSpeed;
            rotation += curAngularVelocity * Time.deltaTime;
            transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, tiltAngle);
        }
        if (Input.GetMouseButton(1)) {
            Vector2 curAngularVelocity = GetInput() * cameraRotateSpeed;
            tiltAngle += curAngularVelocity.x * Time.deltaTime;
            transform.eulerAngles = new Vector3(-rotation.y, rotation.x, tiltAngle);
        }


    }
}
