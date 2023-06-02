using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // 이름을 가지고 찾을 수 있도록 dictionary 클래스 사용해줌
    Dictionary<string,ObjectPool<GameObject>> poolDic;

    private void Awake()
    {
        poolDic = new Dictionary<string,ObjectPool<GameObject>>();  
    }

    // Get으로 꺼내오고 release로 반납

    // 없으면 만들어주고
    // 꽉차면 알아서 지워지도록 구현되어 있음
    // 하...

    // prefab의 이름과 동일하게 네이밍 해주어 헷갈리지 않도록 해준다

    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
    {
        if(original is GameObject)
        {
            // T로 형변환 시켜줌
            GameObject prefab = original as GameObject;

            if (!poolDic.ContainsKey(prefab.name))
                CreatePool(prefab.name, prefab);

            ObjectPool<GameObject> pool = poolDic[prefab.name];
            GameObject go = pool.Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go as T;
            // return 값도 같이 변환
        }
        else if ( original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject go = poolDic[key].Get();
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /*public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDic.ContainsKey(prefab.name))
            CreatePool(prefab.name, prefab);
        
        ObjectPool<GameObject> pool = poolDic[prefab.name];
        GameObject go = pool.Get();
        go.transform.position = position;
        go.transform.rotation = rotation;
        return go;
    }*/

    public T Get<T>(T original, Transform parent) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
    }
    public T Get<T>(T original) where T : Object
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity,null);
    }

    

    public bool Release<T>(T instance) where T : Object
    {
        if (instance is GameObject)
        {
            GameObject go = instance as GameObject;
            string key = go.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(go);
            return true;
        }
        else if (instance is Component)
        {
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsContain<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;

        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    private void CreatePool(string key , GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject go = Instantiate(prefab);
                go.name = key;
                return go;
            },
            actionOnGet: (GameObject go) =>
            {
                go.SetActive(true);
                go.transform.SetParent(null);
            },
            actionOnRelease: (GameObject go) =>
            {
                go.SetActive(false);
                go.transform.SetParent(transform);
            },
            actionOnDestroy: (GameObject go) =>
            {
                Destroy(go);
            }
            );
        poolDic.Add(key,pool);
    }
}
