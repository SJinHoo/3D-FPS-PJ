using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;


    private CharacterController controller;
    private Animator animator;

    private Vector3 moveDir;
    private float moveSpeed;
    private float ySpeed = 0;
    private bool isWalking = false;
    private bool isGrounded;
    private bool isJumping = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        Walk();
    }

    private void Move()
    {   
        // ����������� ���������� ī�޶�� ���� ������ ��� ����� ������
        // controller.Move(moveDir * moveSpeed * Time.deltaTime);

        if(moveDir.magnitude == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
        }

        else if(isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }
        // �ٶ󺸴� ���������� ������
        controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);

        animator.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);
        animator.SetFloat("Speed", moveSpeed);

    }
   

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);

        controller.Move(transform.forward * input.x * moveSpeed * Time.deltaTime);
        controller.Move(transform.right * input.y * moveSpeed * Time.deltaTime);
    }
    private void Walk()
    {
        controller.Move(transform.forward * moveDir.z * walkSpeed * Time.deltaTime);
        controller.Move(transform.right * moveDir.x * walkSpeed * Time.deltaTime);
        animator.SetBool("Walking", isWalking);

        if (Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            isWalking = !isWalking;
            if(isWalking)
            {
                return;
            }
            
        }
    }

    
    
    private void Jump()
    {
        // �߷¹������� ���������� ���� �����ش�
        // ������Ʈ ������ Physics�� gravity �� ������ ������ �����
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (GroundCheck() && ySpeed < 0) 
        {
            ySpeed = -1;
            isJumping = false;
        }
        
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }
    private void OnJump(InputValue value)
    {
        // isground�� ��� ������ �Ϻ����� �ʱ⿡ raycast�� ����
        if (GroundCheck() && !isJumping)
        {
            ySpeed = jumpSpeed;
            animator.SetTrigger("Jump");
            isJumping = true;
        }

    }

    private bool GroundCheck()
    {
        // vector3 origin, �ѷ�, ����, ����
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f, LayerMask.GetMask("Enviroment"));
    }

    
}
