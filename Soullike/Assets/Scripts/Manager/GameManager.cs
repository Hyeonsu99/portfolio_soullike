using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singletone<GameManager>
{
    public GameObject freeLookCamera;
    public GameObject aimCamera;

    private CursorLockMode _cursorLockMode;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursorState(CursorLockMode mode)
    {
        
    }
}
