using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCTalkDataManager", menuName = "PSY/NPCTalkDataManager")]
public class NPCTalkDatabase : ScriptableObject
{
    [SerializeField] private TalkData[] _talks;
    private Dictionary<int, TalkElement[]> talkDatas;

    public void Init()
    {
        talkDatas = new Dictionary<int, TalkElement[]>();
        foreach (TalkData talk in _talks)
        {
            talkDatas.Add(talk.id, talk.talk);
        }
    }

    public TalkElement GetTalk(int talkID, int talkIndex)
    {
        if(talkDatas == null)
        {
            Init();
        }

        if(talkDatas.ContainsKey(talkID))
        {
            if ( talkIndex < talkDatas[talkID].Length)
            {
                return talkDatas[talkID][talkIndex];
            }
        }
        return null;
    }
}

[Serializable]
public class TalkData
{
    [SerializeField] private string name;
    public int id;
    public TalkElement[] talk;
}
[Serializable]
public class TalkElement
{
    public int NPCID;
    public string talk;
}
