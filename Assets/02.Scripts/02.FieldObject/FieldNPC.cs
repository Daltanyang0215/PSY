using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldNPC : MonoBehaviour, IInteraction
{
    public bool IsCanInteraction { get; protected set; }

    [SerializeField] private int _talkID;
    private int _talkIndex;
    private TalkElement _talkElement;

    private TMP_Text _interactionText;

    private void Start()
    {
        _interactionText = GetComponentInChildren<TMP_Text>(true);
    }

    public void OnInteraction()
    {
        _talkElement = GameManager.Instance.TalkDatabase.GetTalk(_talkID, _talkIndex);
        if (_talkElement != null)
        {
            PlayerState.Instance.OnPlayerStop(true);
            _interactionText.text = _talkElement.talk;
            _talkIndex++;
        }
        else
        {
            PlayerState.Instance.OnPlayerStop(false);
            _interactionText.text = "";
            _talkIndex = 0;
        }

    }

    public void OnInteractionZoneEnter()
    {
        _interactionText.gameObject.SetActive(true);
        _interactionText.text = "'Q'를 눌러 대화하기.";
    }

    public void OnInteractionZoneExit()
    {
        _interactionText.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnInteractionZoneEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnInteractionZoneExit();
        }
    }
}
