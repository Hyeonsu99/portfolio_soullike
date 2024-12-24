using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputController _inputController;
    private Animator _animator;

    [SerializeField]
    [Range(0f , 20f)]
    private float _moveSpeed = 2.0f;
    private Quaternion _lastRotation;

    public GameObject freeLookCameraTarget;
    public GameObject aimCameraTarget;

    private float _freeLookCameraTargetYaw;
    private float _freeLookCameraTargetPitch;

    private float _aimCameraTargetYaw;
    private float _aimCameraTargetPitch;

    private float _freeLookCamtopClamp = 70.0f;
    private float _freeLookCambottomClamp = -30.0f;
    private float _cameraAngleOverride = 0.0f;

    private float _aimCamTopClamp = 20f;
    private float _aimCamBottomClamp = -20f;

    private Vector2 _mouseVector;

    private GameObject _freeLookCam;
    private GameObject _aimCam;

    private bool _isReadyCam = false;

    // Start is called before the first frame update
    void Start()
    {
        _freeLookCameraTargetYaw = freeLookCameraTarget.transform.rotation.eulerAngles.y;
        _aimCameraTargetYaw = aimCameraTarget.transform.rotation.eulerAngles.y;

        _freeLookCameraTargetYaw = 0f;
        _freeLookCameraTargetPitch = 0f;

        _inputController = GetComponent<PlayerInputController>();

        _animator = GetComponentInChildren<Animator>();

        StartCoroutine(SettingCameras());

        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator SettingCameras()
    {
        yield return new WaitUntil(() => GameManager.instance.freeLookCamera != null);

        _freeLookCam = GameManager.instance.freeLookCamera;
        _aimCam = GameManager.instance.aimCamera;

        _isReadyCam = true;
    }

    // Update is called once per frame
    void Update()
    {
        _mouseVector = _inputController.mouseInput;

        if(_isReadyCam)
        {
            Move(_inputController.moveInput);

            Aiming();
        }

        // 차후 총기 추가 및 애니메이션 추가되면 수정
        if(Input.GetMouseButtonDown(0) && _inputController.isAiming)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rolling();
        }
    }

    private void LateUpdate()
    {
        if (_inputController.isAiming)
        {
            AimCamRotate();

            freeLookCameraTarget.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            FreeLookCamRotate();
        }
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
        if(_freeLookCam.activeInHierarchy)
        {
            if (moveInput.sqrMagnitude > 0f)
            {
                Vector3 lookForward = new Vector3(freeLookCameraTarget.transform.forward.x, 0f, freeLookCameraTarget.transform.forward.z).normalized;
                Vector3 lookRight = new Vector3(freeLookCameraTarget.transform.right.x, 0f, freeLookCameraTarget.transform.right.z).normalized;

                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized);
              
                if(_inputController.isLock)
                {
                    transform.rotation = _lastRotation;
                }
                else
                {
                    _lastRotation = transform.rotation;

                    transform.rotation = Quaternion.Lerp(transform.rotation, viewRot, 5f * Time.deltaTime);
                }
                
                transform.position += moveDir * _moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.rotation = _lastRotation;
            }
        }
        else if(_aimCam.activeInHierarchy)
        {

            if(moveInput.sqrMagnitude > 0f)
            {
                Vector3 lookForward = new Vector3(aimCameraTarget.transform.forward.x, 0f, aimCameraTarget.transform.forward.z).normalized;
                Vector3 lookRight = new Vector3(aimCameraTarget.transform.right.x, 0f, aimCameraTarget.transform.right.z).normalized;

                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                Quaternion viewRot = Quaternion.LookRotation(moveDir.normalized);

                transform.rotation = Quaternion.Lerp(transform.rotation, viewRot, 5f * Time.deltaTime);

                transform.Translate(moveDir * _moveSpeed * 0.5f * Time.deltaTime);
            }

        }
    }

    private void Aiming()
    {
        if(_inputController.isAiming)
        {
            _freeLookCam.SetActive(false);
            _aimCam.SetActive(true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Quaternion viewRot = Quaternion.LookRotation(new Vector3(ray.direction.x, 0f, ray.direction.z));

            transform.rotation = Quaternion.Lerp(transform.rotation, viewRot, 5f * Time.deltaTime);

            _freeLookCameraTargetPitch = _aimCameraTargetPitch;
            _freeLookCameraTargetYaw = _aimCameraTargetYaw;

            _lastRotation = transform.rotation;
        }
        else
        {
            _freeLookCam.SetActive(true);
            _aimCam.SetActive(false);
        }
    }

    private void FreeLookCamRotate()
    {
        _mouseVector = new Vector2(_mouseVector.x, _mouseVector.y);

        if (_mouseVector.sqrMagnitude > 0.01f)
        {
            float deltaTimeMultiplier = 1.0f;

            _freeLookCameraTargetYaw += _mouseVector.x * deltaTimeMultiplier;
            _freeLookCameraTargetPitch += _mouseVector.y * deltaTimeMultiplier;
        }

        _freeLookCameraTargetYaw = ClampAngle(_freeLookCameraTargetYaw, float.MinValue, float.MaxValue);
        _freeLookCameraTargetPitch = ClampAngle(_freeLookCameraTargetPitch, _freeLookCambottomClamp, _freeLookCamtopClamp);

        freeLookCameraTarget.transform.rotation = Quaternion.Euler(_freeLookCameraTargetPitch + _cameraAngleOverride, _freeLookCameraTargetYaw, 0.0f);
    }

    private void AimCamRotate()
    {
        _mouseVector = new Vector2(_mouseVector.x, _mouseVector.y);

        if (_mouseVector.sqrMagnitude > 0.01f)
        {
            float deltaTimeMultiplier = 1.0f;

            _aimCameraTargetYaw += _mouseVector.x * deltaTimeMultiplier;
            _aimCameraTargetPitch += _mouseVector.y * deltaTimeMultiplier;
        }
        _aimCameraTargetYaw = ClampAngle(_aimCameraTargetYaw, float.MinValue, float.MaxValue);
        _aimCameraTargetPitch = ClampAngle(_aimCameraTargetPitch, _aimCamBottomClamp, _aimCamTopClamp);

        aimCameraTarget.transform.rotation = Quaternion.Euler(_aimCameraTargetPitch + _cameraAngleOverride, _aimCameraTargetYaw, 0.0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360f)
        {
            angle += 360f;
        }

        if(angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }

    private void Fire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(aimCameraTarget.transform.position, ray.direction, Color.red, Mathf.Infinity);

        var obj = ObjectPool.GetObject(ObjectPool.instance.bulletObjectQueue , ObjectPool.instance.bulletObjectPrefab);

        var spawnedBullet = obj.GetComponent<Bullet>();

        spawnedBullet.transform.position = _aimCam.transform.position;
        spawnedBullet.dir = ray.direction;
        spawnedBullet.firePoint = _aimCam.transform; 
        spawnedBullet.ReturnBullet();
    }

    private void Rolling()
    {

    } 
}
