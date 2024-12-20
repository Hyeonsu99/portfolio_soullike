using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Jobs;
using UnityEngine.Timeline;

public class ObjectPool : Singletone<ObjectPool>
{
    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();


    private void Start()
    {
        InitializeObjects(10, poolingObjectPrefab);
    }

    private void InitializeObjects(int count, GameObject obj)
    {
        for(int i = 0; i < count; i++)
        {
            poolingObjectQueue.Enqueue(CreateObject(obj));

            Debug.Log(poolingObjectQueue.Count);
        }
    }

    private GameObject CreateObject(GameObject obj)
    {
        var _newObj = Instantiate(obj);
        _newObj.SetActive(false);
        _newObj.transform.parent = transform;
       
        return _newObj;
    }

    public static GameObject GetObject()
    {
        if(instance.poolingObjectQueue.Count > 0)
        {
            var _obj = instance.poolingObjectQueue.Dequeue();
            _obj.transform.SetParent(null);
            _obj.SetActive(true);

            return _obj;
        }
        else
        {
            var _newObj = instance.CreateObject(instance.poolingObjectPrefab);
            _newObj.transform.SetParent(null);
            _newObj.SetActive(true);

            return _newObj;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(obj);
    }
}
