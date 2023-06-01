using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float maxDistance;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;

    
    public void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hit, maxDistance))
        {
            IHittable hittable = hit.transform.GetComponent<IHittable>();
            ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            effect.transform.parent = hit.transform.transform;
            Destroy(effect.gameObject, 3f );

            
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));
            Destroy(effect.gameObject, 3f);
            hittable?.Hit(hit, damage);
            
            //TrailRenderer 
        }

        else
        {
            
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
            
        }

        IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
        {
            TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

            float time = 0;
            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, time);
                time += Time.deltaTime / totalTime;
                
                yield return null;
            }

            Destroy(trail.transform.gameObject, 3f);
        }
    }
}
