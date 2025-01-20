using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float lookSpeedVertical = 1.0f;
    public float lookSpeedHorizontal = 1.0f;
    public GameObject head = null;

    public struct PickedupInfo
    {
        public GameObject gameObject;
        public Vector3 startHandle;
        public Vector3 startPosition;
    }

    public PickedupInfo? pickedUp = null;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Assert(head != null, "Cannot create a Player Controller with no head");
    }

    void Update()
    {
        UpdateMovement();
        UpdateLook();
        UpdatePickup();
    }

    private void UpdatePickup()
    {
        const int LeftButton = 0;
        if (Input.GetMouseButtonDown(LeftButton))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var gameObject = hit.collider.gameObject;
                pickedUp = new PickedupInfo
                {
                    gameObject = gameObject,
                    startHandle = hit.point,
                    startPosition = gameObject.transform.position,
                };
            }
            else
            {
                pickedUp = null;
            }
        }
        else if (Input.GetMouseButtonUp(LeftButton))
        {
            pickedUp = null;
        }

        if (pickedUp is PickedupInfo info)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var groundPlane = new Plane(Vector3.up, info.startHandle);

            if (groundPlane.Raycast(ray, out var enter))
            {
                var point = ray.GetPoint(enter);
                info.gameObject.transform.position = info.startPosition + ray.GetPoint(enter) - info.startHandle;
            }
            else
            {
                info.gameObject.transform.position = info.startPosition;
            }
        }
    }

    private void UpdateMovement()
    {
        float foreward = 0f;
        float sideward = 0f;

        foreward += Input.GetKey(KeyCode.W) ? 1f : 0f;
        foreward -= Input.GetKey(KeyCode.S) ? 1f : 0f;

        sideward -= Input.GetKey(KeyCode.A) ? 1f : 0f;
        sideward += Input.GetKey(KeyCode.D) ? 1f : 0f;

        var movement = new Vector3(sideward, 0f, foreward);
        movement.Normalize();
        movement = moveSpeed * movement;
        transform.Translate(movement);
    }

    private void UpdateLook()
    {
        float mouseMoveX = Input.GetAxis("Mouse X");
        float mouseMoveY = Input.GetAxis("Mouse Y");

        head.transform.Rotate(
            -lookSpeedVertical * mouseMoveY / (2f * (float)Math.PI),
            0f,
            0f
        );

        transform.Rotate(
            0f,
            lookSpeedHorizontal * mouseMoveX / (2f * (float)Math.PI),
            0f
        );
    }
}
