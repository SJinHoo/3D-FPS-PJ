using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private float reloadTime;
    [SerializeField] WeaponHolder weaponHolder;



    private Animator animator;
    private bool isReloading;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void OnReload(InputValue value)
    {
        StartCoroutine(ReloadRoutine());
    }

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        Fire();
    }

    IEnumerator ReloadRoutine()
    {
        animator.SetTrigger("Reload");
        isReloading = true;
        aimRig.weight = 0f;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        aimRig.weight = 1.0f;

    }

    private void Fire()
    {
        weaponHolder.Fire();
        animator.SetTrigger("Fire");
    }
}
