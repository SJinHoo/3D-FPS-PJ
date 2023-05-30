using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;


    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed = 0;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {   
        // 월드기준으로 움직임으로 카메라와 같이 적용할 경우 어색한 움직임
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // 바라보는 뱡항으로의 움직임
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }
   

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        // 중력방향으로 지속적으로 힘을 가해준다
        // 프로젝트 세팅의 Physics의 gravity 로 설정된 값으로 적용됨
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0) 
        {
            ySpeed = -1;
        }

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }
    private void OnJump(InputValue value)
    {
        // isground의 경우 구현이 완벽하지 않기에 raycast로 구현
        if(GroundCheck())
            ySpeed = jumpSpeed;
    }

    private bool GroundCheck()
    {
        // vector3 origin, 둘레, 방향, 길이
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f, LayerMask.GetMask("Enviroment"));
    }

    
}
