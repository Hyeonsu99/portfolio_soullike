using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_BattleScene : MonoBehaviour
{
    PlayerController _playerController;

    Boss_0 _boss_0;

    public TextMeshProUGUI magazineText;

    public Slider hpImage;
    public Slider spImage;

    public Slider bossHpImage;

    private void OnEnable()
    {
        _playerController = FindAnyObjectByType<PlayerController>();

        _boss_0 = FindAnyObjectByType<Boss_0>();

        _playerController.magazineChangeEvent += MagazineChange;
        _playerController.hpChangeEvent += PlayerHpChangeEvent;

        _boss_0.hpChangeEvent += BossHpChangeEvent;
    }

    private void MagazineChange()
    {
        magazineText.text = $"{_playerController.currentGun.curMagazine} / {_playerController.currentGun.maxMagazine}";
    }

    private void PlayerHpChangeEvent()
    {

    }

    private void BossHpChangeEvent(float hp)
    {
        bossHpImage.value = hp / _boss_0.maxHp;
    }
}
