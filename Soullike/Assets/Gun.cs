using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int maxMagazine = 6;
    public int curMagazine;

    public float damage;

    public Transform firePoint;

    private Animator _animator;

    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        curMagazine = maxMagazine;

        _animator = GetComponentInChildren<Animator>();
        
        _playerController = GetComponentInParent<PlayerController>();

        _playerController.shootEvent += Fire;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
