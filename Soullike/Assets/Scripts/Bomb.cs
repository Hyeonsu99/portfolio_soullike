using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public const string DESTROY_THIS_NAME = "Destroythis";


    // Start is called before the first frame update
    void Start()
    {
        Invoke(DESTROY_THIS_NAME, 3f);
    }

    private void Destroythis()
    {
        ObjectPool.ReturnObject(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
