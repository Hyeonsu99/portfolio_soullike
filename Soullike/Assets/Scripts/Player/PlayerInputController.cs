using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 moveInput;

    public Vector2 mouseInput;

    public bool isLock;

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
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        isFire = Input.GetButtonDown("Fire1");
        isAiming = Input.GetButton("Fire2");

        isRolling = Input.GetKeyDown(KeyCode.LeftControl);

        isLock = Input.GetKey(KeyCode.LeftAlt);
    }
}
