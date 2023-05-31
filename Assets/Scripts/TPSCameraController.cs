using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float CameraSensitivity;
    [SerializeField] private float lookDistance;
    [SerializeField] Transform aimTarget;


    
    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Rotate();
    }
    private void LateUpdate()
    {
        Look();
    }

    private void Rotate()
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y;
        transform.LookAt(lookPoint);
    }


    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity;
        xRotation -= lookDelta.y * mouseSensitivity;
        // Mathf.Clamp( value, min , max)
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);


        // 플레이어가 바라보는 방향과 관계없이 회전을 하기위해 local rotation 대신 rotation을 써준다.
        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        
    }



    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
