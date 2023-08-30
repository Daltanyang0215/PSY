using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FieldPushButton : MonoBehaviour
{
    public bool isPressed { get; private set; }

    private enum PressType
    {
        None,
        Push,               // 눌림 고정
        Press,              // 누르고 있어야됨
        UnPress,            // 눌리면 반대로
        OnlyPlayerPush,     // 플레이어 한정 한번만 누름
        OnlyPlayerPress     // 플레이어 한정 누르고 있어야됨
    }

    [SerializeField] private PressType _pressType;

    [Tooltip("마우스 조작으로만 실행되는 물건 인지 지정")]
    [SerializeField] private bool _isOnlyClick;

    public void OnPSYClick()
    {
        if (!_isOnlyClick) return;

        if(_pressType == PressType.Push)
        {
            isPressed = true;
        }
        else if( _pressType == PressType.Press)
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
                default:
                    break;
            }
        }
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
                default:
                    break;
            }
        }
    }
}
