using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firePoint;

    Vector3 dir;

    Ray ray;

    public const string DESTROY_THIS_NAME = "Destroythis";

    // Start is called before the first frame update
    void Start()
    {
        transform.position = firePoint.position;

        dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;

        ray = new Ray(firePoint.position, dir);

        Invoke(DESTROY_THIS_NAME, 5f);
    }

    private void Destroythis()
    {
        ObjectPool.ReturnObject(gameObject);
    }

    private void Update()
    {
        transform.position += ray.direction * 50f * Time.deltaTime;
    }
}
