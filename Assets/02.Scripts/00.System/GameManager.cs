using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("MainGameManager").GetComponent<GameManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    [SerializeField] private NPCTalkDatabase talkDatabase;
    public NPCTalkDatabase TalkDatabase => talkDatabase;

    public FieldNPC CurTalkNPC { get; set; }

    [SerializeField] private GameProgress progress;
    public int CurEventID => progress.curEventID;

    public void AddEventID(int index)
    {
        progress.curEventID += index;
    }
    public void SetEventID(int index)
    {
        progress.curEventID = index;
    }
}
