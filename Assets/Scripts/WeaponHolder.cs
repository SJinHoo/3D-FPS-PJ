using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] Gun gun;

    // List<Gun> gunList = new List<Gun>();
    // 여러 종류의 총을 가지고 있는 경우 리스트에 넣어 꺼내어 씀

    public void Fire()
    {
        gun.Fire();
    }
}
