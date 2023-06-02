using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{


    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] Poolable poolablePrefab;

        [SerializeField] int poolSize;
        [SerializeField] int maxSize;

        private Stack<Poolable> objectPool = new Stack<Poolable>();

        // 외부사항에 
        private void Awake()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.gameObject.SetActive(false);
                poolable.transform.SetParent(transform);
                objectPool.Push(poolable);
            }
        }

        public Poolable Get()
        {
            if (objectPool.Count > 0)
            {
                Poolable poolable = objectPool.Pop();
                poolable.gameObject.SetActive(true);
                poolable.transform.parent = null;
                return poolable;
            }
            else
            {
                Poolable poolable = Instantiate(poolablePrefab);
                poolable.Pool = this;
                return poolable;
            }
        }

        public void Release(Poolable poolable)
        {
            if (objectPool.Count < maxSize)
            {
                poolable.gameObject.SetActive(false);
                poolable.transform.SetParent(transform);
                poolable.Pool = this; // 반납할 위치 설정
                objectPool.Push(poolable);
            }
            // 정해놓은 사이즈가 초과될경우에는 poolable 을 삭제
            else
            {
                Destroy(poolable.gameObject);
            }

        }
    }
}
