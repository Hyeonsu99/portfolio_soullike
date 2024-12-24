using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firePoint;

    public Vector3 dir;

    public const string DESTROY_THIS_NAME = "Destroythis";


    public void ReturnBullet()
    {
        Invoke(DESTROY_THIS_NAME, 5f);
    }

    private void Destroythis()
    {
        ObjectPool.ReturnObject(gameObject);
    }

    private void Update()
    {
        transform.position += dir * 50f * Time.deltaTime;
    }
}
