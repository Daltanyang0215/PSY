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
            if(instance == null)
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
    
}
