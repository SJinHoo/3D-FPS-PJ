using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private Rig rig;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void OnReload(InputValue value)
    {
        animator.SetTrigger("Reload");       
    }

    private void OnFire(InputValue value)
    {
        animator.SetTrigger("Fire");
    }
}
