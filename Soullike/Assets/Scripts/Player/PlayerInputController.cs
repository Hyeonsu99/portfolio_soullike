using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 moveInput;

    public Vector2 mouseInput;

    public Vector2 mousePosition;

    public bool isLock;

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

        mousePosition = Input.mousePosition;

        isAiming = Input.GetButton("Fire2");

        isLock = Input.GetKey(KeyCode.LeftAlt);
    }
}
