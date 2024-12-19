using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform firePoint;

    Vector3 dir;

    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = GameObject.Find("AimCamera").transform;

        dir = Camera.main.ScreenPointToRay(Input.mousePosition).direction;

        ray = new Ray(firePoint.position, dir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += ray.direction * 50f * Time.deltaTime;
    }
}
