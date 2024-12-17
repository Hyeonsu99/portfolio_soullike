using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputController _inputController;
    private Animator _animator;

    private float _moveSpeed = 2f;

    public GameObject freeLookCam;
    public GameObject aimingCam;

    // Start is called before the first frame update
    void Start()
    {
        _inputController = GetComponent<PlayerInputController>();

        _animator = GetComponentInChildren<Animator>();

        freeLookCam.SetActive(true);
        aimingCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Move(_inputController.moveInput);

        Aiming();

        Rolling();

        Fire();
    }

    // 지속적으로 업데이트 되는 애니메이션에는 사용 불가능
    // 구르기, 공격 등 단발성 애니메이션에 사용
    bool IsAnimationRunning(string stateName)
    {
        if (_animator != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime > 1f && normalizedTime != 0;
            }
        }

        return false;
    }

    private void Move(Vector2 moveInput)
    {
        Vector3 inputVec = new Vector3(moveInput.x, 0, moveInput.y);

        if(moveInput.sqrMagnitude > 0)
        {
            Vector3 moveVec = Vector3.forward * _moveSpeed * Time.deltaTime;
            transform.Translate(moveVec);

            Vector3 normalVec = inputVec.normalized;
            Quaternion rot = Quaternion.identity;

            rot.eulerAngles = new Vector3(0, Mathf.Atan2(normalVec.x, normalVec.z) * Mathf.Rad2Deg, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }   
    }

    private void Aiming()
    {
        if(_inputController.isAiming)
        {
            freeLookCam.SetActive(false);
            aimingCam.SetActive(true);
        }
        else
        {
            freeLookCam.SetActive(true);
            aimingCam.SetActive(false);
        }

    }

    private void Fire()
    {

    }

    private void Rolling()
    {

    } 
}
