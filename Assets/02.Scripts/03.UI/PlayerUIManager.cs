using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private Slider _playerHpSlider;
    [SerializeField] private Slider _playerMpSlider;

    private void Update()
    {
        UpdataHpUI();
        UpdataMpUI();
    }

    public void UpdataHpUI()
    {
        if (_playerHpSlider.value != PlayerState.Instance.PlayerHpUI)
            _playerHpSlider.value = PlayerState.Instance.PlayerHpUI;
    }
    public void UpdataMpUI()
    {
        if (_playerMpSlider.value != PlayerState.Instance.PlayerMpUI)
            _playerMpSlider.value = PlayerState.Instance.PlayerMpUI;
    }
}
