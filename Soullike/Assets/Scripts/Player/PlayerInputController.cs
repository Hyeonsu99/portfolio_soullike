using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 moveInput;

    
    public bool isRolling;
    public bool isFire;
    public bool isAiming;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        isFire = Input.GetButtonDown("Fire1");
        isAiming = Input.GetButton("Fire2");
        isRolling = Input.GetButtonDown("Jump");
    }
}
