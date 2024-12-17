using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    private float Yaxis;
    private float Xaxis;

    private float rotSensitive = 3f;

    private float _rotationMin = -10;
    private float _rotationMax = 80;
    private float _smoothTime = 0.12f;

    private Vector3 targetRotation;
    private Vector3 currrentVel;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        Vector3 offsetPosition = target.position + positionOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, offsetPosition, 180f * Time.deltaTime);

        Quaternion offsetRotation = rotationOffset;
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, offsetRotation, 30f * Time.deltaTime);

        transform.position = smoothPosition;
        transform.rotation = smoothRotation;
    }
}
