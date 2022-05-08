using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 10.0f;

    private Camera _camera;
    private bool isMouseDragging = false;
    private bool isTilting = false;
    private Vector2 mouseDragOrigin;
    private Vector2 mouseTiltOrigin;

    private void TranslateTo(Vector3 direction) {
        transform.Translate(direction * cameraMoveSpeed * Time.deltaTime);
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

        if (Input.GetMouseButtonDown(0)) {
            isMouseDragging = true;
            mouseDragOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) {
            isMouseDragging = false;
        }
        if (isMouseDragging) {
            Vector2 mouseDragEnd = Input.mousePosition;
            Vector2 delta = mouseDragEnd - mouseDragOrigin;
            transform.Rotate(Vector3.up, delta.x * Time.deltaTime * cameraMoveSpeed);
            transform.Rotate(Vector3.right, -delta.y * Time.deltaTime * cameraMoveSpeed);
            mouseDragOrigin = mouseDragEnd;
        }

        if (Input.GetMouseButtonDown(1)) {
            isTilting = true;
            mouseTiltOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1)) {
            isTilting = false;
        }
        if (isTilting) {
            Vector2 mouseTiltEnd = Input.mousePosition;
            Vector2 delta = mouseTiltEnd - mouseTiltOrigin;
            transform.Rotate(Vector3.forward, -delta.x * Time.deltaTime * cameraMoveSpeed);
            mouseTiltOrigin = mouseTiltEnd;
        }


    }
}
