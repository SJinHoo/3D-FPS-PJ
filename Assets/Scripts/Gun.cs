using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] float maxDistance;
    [SerializeField] float bulletSpeed;
    [SerializeField] int damage;

    

    public void Fire()
    {
        muzzleEffect.Play();

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out hit, maxDistance))
        {
            // 맞을 수 있는 물체를 판별하는 코드
            // IHittalble 인터페이스 생성하고 getcomponet 해주어 맞을 수 있는 객체를 선정해줌
            IHittable hittable = hit.transform.GetComponent<IHittable>();

            
            ParticleSystem effect = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/HitEffect", hit.point, Quaternion.LookRotation(hit.normal), true);
            effect.transform.parent = hit.transform.transform;
            StartCoroutine(ReleaseRoutine(effect.gameObject));

            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);
            
            
        }

        else
        {            
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));           
        }

        IEnumerator ReleaseRoutine(GameObject effect)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Resource.Destroy(effect.gameObject);
        }

        IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
        {
            // Pooling 한 오브젝트는 반드시 초기화를 시켜주어야 함
            TrailRenderer trail = GameManager.Resource.Instantiate<TrailRenderer>("Prefabs/BulletTrail", muzzleEffect.transform.position, Quaternion.identity,true);          
            trail.Clear();
            float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

            float rate = 0;
            while (rate < 1)
            {
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
                rate += Time.deltaTime / totalTime;
                
                yield return null;
            }

            GameManager.Resource.Destroy(trail.gameObject);
        }
    }
}
