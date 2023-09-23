using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FieldPushButton : FieldEventTrigger, IInteraction
{
    private enum PressType
    {
        None,
        Push,               // 눌림 고정
        Press,              // 누르고 있어야됨
        UnPress,            // 눌리면 반대로
        OnlyPlayerPush,     // 플레이어 한정 한번만 누름
        OnlyPlayerPress,    // 플레이어 한정 누르고 있어야됨
        OnInteraction
    }

    [SerializeField] private PressType _pressType;

    [Tooltip("마우스 조작으로만 실행되는 물건 인지 지정")]
    [SerializeField] private bool _isOnlyClick;

    private Transform _textTransform;
    public bool IsCanInteraction => _pressType == PressType.OnInteraction;

    private void Start()
    {
        if (IsCanInteraction)
        {
            _textTransform = transform.GetChild(0);
            _textTransform.gameObject.SetActive(false);
        }
    }

    public void OnPSYClick()
    {
        if (!_isOnlyClick) return;

        if (_pressType == PressType.Push)
        {
            isPressed = true;
        }
        else if (_pressType == PressType.Press)
        {
            isPressed = !isPressed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !_isOnlyClick)
        {
            switch (_pressType)
            {
                case PressType.None:
                    break;
                case PressType.Push:
                case PressType.Press:
                    isPressed = true;
                    break;
                case PressType.UnPress:
                    isPressed = false;
                    break;
                case PressType.OnlyPlayerPush:
                case PressType.OnlyPlayerPress:
                    if (collision.CompareTag("Player"))
                    {
                        isPressed = true;
                    }
                    break;
                case PressType.OnInteraction:
                    if (collision.CompareTag("Player"))
                        OnInteractionZoneEnter();
                    break;
                default:
                    break;
            }
        }
        _animator?.SetBool("OnPress", isPressed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && !_isOnlyClick)
        {
            switch (_pressType)
            {
                case PressType.None:
                case PressType.Push:
                    break;
                case PressType.Press:
                    isPressed = false;
                    break;
                case PressType.UnPress:
                    isPressed = true;
                    break;
                case PressType.OnlyPlayerPush:
                    break;
                case PressType.OnlyPlayerPress:
                    if (collision.CompareTag("Player"))
                    {
                        isPressed = false;
                    }
                    break;
                case PressType.OnInteraction:
                    if (collision.CompareTag("Player"))
                        OnInteractionZoneExit();
                    break;
                default:
                    break;
            }
        }
        _animator?.SetBool("OnPress", isPressed);
    }

    public void OnInteractionZoneEnter()
    {
        _textTransform.gameObject.SetActive(true);
    }

    public void OnInteraction()
    {
        isPressed = true;
        _pressType = PressType.None;
        OnInteractionZoneExit();
    }

    public void OnInteractionZoneExit()
    {
        _textTransform.gameObject.SetActive(false);
    }
}
