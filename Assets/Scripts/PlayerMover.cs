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
        // ����������� ���������� ī�޶�� ���� ������ ��� ����� ������
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // �ٶ󺸴� ���������� ������
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
        // �߷¹������� ���������� ���� �����ش�
        // ������Ʈ ������ Physics�� gravity �� ������ ������ �����
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0) 
        {
            ySpeed = -1;
        }

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }
    private void OnJump(InputValue value)
    {
        // isground�� ��� ������ �Ϻ����� �ʱ⿡ raycast�� ����
        if(GroundCheck())
            ySpeed = jumpSpeed;
    }

    private bool GroundCheck()
    {
        // vector3 origin, �ѷ�, ����, ����
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f, LayerMask.GetMask("Enviroment"));
    }

    
}
