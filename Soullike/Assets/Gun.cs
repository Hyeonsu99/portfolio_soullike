using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxMagazine = 6;
    public int curMagazine;

    public float damage;

    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        curMagazine = maxMagazine;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
