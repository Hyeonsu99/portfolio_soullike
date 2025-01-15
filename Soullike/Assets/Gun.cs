using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Revolver,
        Rifle
    }

    public GunType type;    

    public int maxMagazine = 6;
    public int curMagazine;

    public float damage;

    public Transform firePoint;

    public Animator _animator;

    private PlayerController _playerController;

    // Start is called before the first frame update


    private void Awake()
    {
        curMagazine = maxMagazine;

        _animator = GetComponentInChildren<Animator>();

        _playerController = GetComponentInParent<PlayerController>();

        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitUntil(() => _playerController != null);   
    }

    private void OnEnable()
    {
        _playerController.currentGun = this;

        _playerController.shootEvent += Fire;

        if (_playerController.animator != null)
        {
            _playerController.animator.SetInteger("GunType", (int)type);
        }
    }

    private void OnDisable()
    {
        _playerController.shootEvent -= Fire;
    }

    private void Fire()
    {
        if(curMagazine > 0)
        {
            curMagazine--;

            _animator.SetTrigger("Shoot");
        }
    }

    public void Reload()
    {
        curMagazine = maxMagazine;
    }

    private IEnumerator Reload(float time)
    {
        yield return new WaitForSeconds(time);

        curMagazine = maxMagazine;
    }
}
