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
    [SerializeField] private float repeatTime;
    [SerializeField] WeaponHolder weaponHolder;
    public TrailRenderer bulletTrail;

    
    private Animator animator;
    private bool isReloading;
    

    private void Awake()
    {
        bulletTrail = Resources.Load<TrailRenderer>("Prefabs/BulletTrail");
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        
    }
    private void OnReload(InputValue value)
    {
        StartCoroutine(ReloadRoutine());
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

    private void OnFire(InputValue value)
    {
        if (isReloading)
            return;

        Fire();
    }

    private void Fire()
    {
        weaponHolder.Fire();
        animator.SetTrigger("Fire");
    }

    private void OnRapidFire(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("연사");
            RFroutine = StartCoroutine(RapidFireRoutine());
        }
        else
        {
            Debug.Log("연사 중지");
            StopCoroutine(RFroutine);
        }

    }
    public Coroutine RFroutine;
    IEnumerator RapidFireRoutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(repeatTime);
        }
    }
}
