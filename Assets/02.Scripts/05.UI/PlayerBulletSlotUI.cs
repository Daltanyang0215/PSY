using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBulletSlotUI : MonoBehaviour
{
    [SerializeField] private PSYGrap _bulletdata;

    [SerializeField] private Image _mainBulletImage;
    [SerializeField] private TMP_Text _mainBulletCount;

    [SerializeField] private Image _upBulletImage;
    [SerializeField] private TMP_Text _upBulletCount;

    [SerializeField] private Image _downBulletImage;
    [SerializeField] private TMP_Text _downBulletCount;

    private void Update()
    {
        OnUpdateUI();
    }

    public void OnUpdateUI()
    {
        int bulletslotindex = _bulletdata.BulletSelect;

        _mainBulletImage.sprite = _bulletdata.BulletObjects[bulletslotindex][0].GetComponent<SpriteRenderer>().sprite;
        _mainBulletCount.text = _bulletdata.BulletObjects[bulletslotindex].Count.ToString("00");

        bulletslotindex = _bulletdata.BulletSelect == _bulletdata.BulletObjects.Count-1 ? 0 : _bulletdata.BulletSelect + 1;

        _upBulletImage.sprite = _bulletdata.BulletObjects[bulletslotindex][0].GetComponent<SpriteRenderer>().sprite;
        _upBulletCount.text = _bulletdata.BulletObjects[bulletslotindex].Count.ToString("00");

        bulletslotindex = _bulletdata.BulletSelect == 0 ? _bulletdata.BulletObjects.Count - 1 : _bulletdata.BulletSelect - 1;
        _downBulletImage.sprite = _bulletdata.BulletObjects[bulletslotindex][0].GetComponent<SpriteRenderer>().sprite;
        _downBulletCount.text = _bulletdata.BulletObjects[bulletslotindex].Count.ToString("00");

    }

}