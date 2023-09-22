using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYGrap", menuName = "PSYSkill/PSYGrap")]

public class PSYGrap : PSYSkillBase
{
    [SerializeField] private float _psyPowerMut;
    [SerializeField] private KinesisObjectBase _psyBulletPrefab;
    [SerializeField] private int _psyBulletInitCount;

    private int _bulletSelect;
    public int BulletSelect => _bulletSelect;

    //private List<List<KinesisObjectBase>> _bulletObjects = new List<List<KinesisObjectBase>>();
    private List<List<KinesisObjectBase>> _bulletObjects;
    public List<List<KinesisObjectBase>> BulletObjects => _bulletObjects;

    public void DefualtBulletInit()
    {
        for (int i = _psyBulletInitCount - _bulletObjects[0].Count; i > 0; i--)
        {
            KinesisObjectBase addbuulet = Instantiate(_psyBulletPrefab, Vector3.zero, Quaternion.identity);
            addbuulet.SetOrder(OrderType.Player, PSYID);
            addbuulet.gameObject.SetActive(false);
            _bulletObjects[0].Add(addbuulet);
        }
    }

    public override void OnPSYInit()
    {

        //kinesisObjects.Clear();
        //_psyBullets = new Stack<KinesisObjectBase>();
        //for (int i = 0; i < _psyBulletInitCount; i++)
        //{
        //    KinesisObjectBase addbuulet = Instantiate(_psyBulletPrefab, Vector3.zero, Quaternion.identity);
        //    addbuulet.SetOrder(OrderType.Player,PSYID);
        //    addbuulet.gameObject.SetActive(false);
        //    _psyBullets.Push(addbuulet);
        //}
        PlayerState.Instance.PlayerRecoveryAction += () => DefualtBulletInit();

        _bulletSelect = 0;
        _bulletObjects = PlayerState.Instance.bulletObjects;
        _bulletObjects.Clear();

        List<KinesisObjectBase> addBulletPF = new List<KinesisObjectBase>();
        for (int i = 0; i < _psyBulletInitCount; i++)
        {
            KinesisObjectBase addbuulet = Instantiate(_psyBulletPrefab, Vector3.zero, Quaternion.identity);
            addbuulet.SetOrder(OrderType.Player, PSYID);
            addbuulet.gameObject.SetActive(false);
            addBulletPF.Add(addbuulet);
        }
        _bulletObjects.Add(addBulletPF);
    }

    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _bulletSelect++;
            if (_bulletSelect >= _bulletObjects.Count)
            {
                _bulletSelect = 0;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _bulletSelect--;
            if (_bulletSelect < 0)
            {
                _bulletSelect = _bulletObjects.Count - 1;
            }
        }
    }

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        //Collider2D enemybullet = Physics2D.OverlapCircle(point, 0.1f, PlayerState.Instance.AttackTargetLayer);

        //if (enemybullet != null
        //    && enemybullet.TryGetComponent(out KinesisObjectBase target)
        //    && PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip))
        //{
        //    target.StopPSYForce(true);
        //    target.SetOrder(OrderType.Player);
        //    target.SetPSYPranet(PlayerState.Instance.transform);
        //    kinesisObjects.Add(target);
        //}
        //else if (kinesisObjects.Count > 0)
        //{
        //    PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
        //    kinesisObjects[0].SetPSYPranet(null);
        //    kinesisObjects[0].SetPSYForce((point - (PlayerState.Instance.transform.position + Vector3.up)).normalized * PSYLevel * _psyPowerMut);
        //    kinesisObjects.RemoveAt(0);
        //}
        //else if(_psyBullets.Count > 0)
        //{
        //    KinesisObjectBase shotBullet = _psyBullets.Pop();
        //    shotBullet.transform.position = PlayerState.Instance.transform.position+Vector3.up;
        //    shotBullet.gameObject.SetActive(true);
        //    shotBullet.AddPSYForce((point - (PlayerState.Instance.transform.position + Vector3.up)).normalized * PSYLevel * _psyPowerMut*2f);
        //}

        Collider2D enemybullet = Physics2D.OverlapCircle(point, 0.1f, PlayerState.Instance.AttackTargetLayer);
        //잡을때
        if (enemybullet != null
            && enemybullet.TryGetComponent(out KinesisObjectBase target)
            && target.ObjectType == KinesisObjectType.Bullet)
        {
            //기본 총알은 예외처리
            if (target.objectID == 1)
            {
                target.SetOrder(OrderType.Player, PSYID);
                target.gameObject.SetActive(false);
                _bulletObjects[0].Add(target);
                return;
            }
            if (!PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip)) return;

            // 오브젝트 리스트에서 동일한 아이디를 지닌 리스트가 있는지 확인
            for (int i = 1; i < _bulletObjects.Count; i++)
            {
                //있으면 기존 리스트에 추가
                if (_bulletObjects[i][0].objectID == target.objectID)
                {
                    target.StopPSYForce(true);
                    target.SetOrder(OrderType.Player);
                    target.SetPSYPranet(PlayerState.Instance.transform);
                    _bulletObjects[i].Add(target);

                    return;
                }
            }
            //foreach (List<KinesisObjectBase> serchlist in _bulletObjects)
            //{
            //    //있으면 기존 리스트에 추가
            //    if (serchlist[0].objectID == target.objectID)
            //    {
            //        target.StopPSYForce(true);
            //        target.SetOrder(OrderType.Player);
            //        target.SetPSYPranet(PlayerState.Instance.transform);
            //        serchlist.Add(target);

            //        return;
            //    }
            //}
            //없으면 리스트를 생성하여 오브젝트 리스트에 추가
            List<KinesisObjectBase> addBullet = new List<KinesisObjectBase>();
            target.StopPSYForce(true);
            target.SetOrder(OrderType.Player);
            target.SetPSYPranet(PlayerState.Instance.transform);
            addBullet.Add(target);
            _bulletObjects.Add(addBullet);
        }
        //쏠때
        else
        {
            // 0번. 기본 총알이면 마나 회수 안됨
            if (_bulletSelect != 0) PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
            // 기본 총알 선택 중 총알 없으면 리턴
            if (_bulletSelect == 0)
            {
                if (_bulletObjects[_bulletSelect].Count == 0)
                    return;

                _bulletObjects[_bulletSelect][0].gameObject.SetActive(true);
                _bulletObjects[_bulletSelect][0].transform.position = PlayerState.Instance.transform.position + Vector3.up;
            }

            _bulletObjects[_bulletSelect][0].SetPSYPranet(null);
            _bulletObjects[_bulletSelect][0].SetPSYForce((point - (PlayerState.Instance.transform.position + Vector3.up)).normalized * PSYLevel * _psyPowerMut);
            _bulletObjects[_bulletSelect].RemoveAt(0);

            // 기본 총알 제외 선택 중 총알 없으면 리스트 정리
            if (_bulletSelect != 0 && _bulletObjects[_bulletSelect].Count == 0)
            {
                _bulletObjects.RemoveAt(_bulletSelect);
                _bulletSelect--;
            }
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer) { }
    public override void OnPSYExit(Vector3 point, LayerMask targetlayer) { }
}
