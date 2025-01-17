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
    public GameObject bulletObjectPrefab;

    [SerializeField]
    public GameObject bombObjectPrefab;

    public Queue<GameObject> bulletObjectQueue = new Queue<GameObject>();
    public Queue<GameObject> bombObjectQueue = new Queue<GameObject>();


    private void Start()
    {
        InitializeObjects(10, bulletObjectQueue, bulletObjectPrefab);

        InitializeObjects(4, bombObjectQueue, bombObjectPrefab);
    }

    private void InitializeObjects(int count, Queue<GameObject> queue, GameObject obj)
    {
        for(int i = 0; i < count; i++)
        {
            queue.Enqueue(CreateObject(obj));
        }
    }

    private GameObject CreateObject(GameObject obj)
    {
        var _newObj = Instantiate(obj);
        _newObj.SetActive(false);
        _newObj.transform.parent = transform;
       
        return _newObj;
    }

    public static GameObject GetObject(Queue<GameObject> queue,GameObject obj)
    {
        if(queue.Count > 0)
        {
            var _obj = queue.Dequeue();
            _obj.transform.SetParent(null);
            _obj.SetActive(true);

            return _obj;
        }
        else
        {
            var _newObj = instance.CreateObject(obj);
            _newObj.transform.SetParent(null);
            _newObj.SetActive(true);

            return _newObj;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.bulletObjectQueue.Enqueue(obj);
    }
}
