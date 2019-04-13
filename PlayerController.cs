using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputMode
{
    Keyboard,
    Mouse
}

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    InputMode m_InputMode = InputMode.Keyboard;

    [SerializeField]
    SphereControlled m_Controlled = null;

    Vector3 m_MouseInputPositionLastFrame = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Controlled)
        {
            switch (m_InputMode)
            {
                case InputMode.Keyboard:
                    ProcessKeyboardInput();
                    break;
                case InputMode.Mouse:
                    ProcessMouseInput();
                    break;
                default:
                    break;
            }
        }
    }

    void ProcessKeyboardInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Controlled.TurnRight(-1f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Controlled.TurnRight(1f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            m_Controlled.Roll(-1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_Controlled.Roll(1f);
        }
    }

    void ProcessMouseInput()
    {/*
        if (Input.GetMouseButtonDown(0))
        {
            m_Controlled.CanRotate = false;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = Input.mousePosition - m_MouseInputPositionLastFrame;

            bool isRight = Vector3.Dot(direction)

            m_Controlled.TurnRight()

            m_MouseInputPositionLastFrame = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_Controlled.CanRotate = true;
        }*/
    }
}
