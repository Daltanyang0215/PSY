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
        Push,               // ���� ����
        Press,              // ������ �־�ߵ�
        UnPress,            // ������ �ݴ��
        OnlyPlayerPush,     // �÷��̾� ���� �ѹ��� ����
        OnlyPlayerPress     // �÷��̾� ���� ������ �־�ߵ�
    }

    [SerializeField] private PressType _pressType;

    [Tooltip("���콺 �������θ� ����Ǵ� ���� ���� ����")]
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
