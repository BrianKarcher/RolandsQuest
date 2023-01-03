using RQ.Input.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ
{
    [Serializable]
	public class RawInput
	{
		public Vector2 axisInput;
        private Dictionary<Button, ButtonState> _buttonState;
        //private bool[] buttonDown;
        //private bool[] buttonPressed;

		public RawInput()
		{
            _buttonState = new Dictionary<Button, ButtonState>();
            _buttonState.Add(Button.Primary, new ButtonState());
            _buttonState.Add(Button.Secondary, new ButtonState());
            _buttonState.Add(Button.Special, new ButtonState());
            _buttonState.Add(Button.Menu, new ButtonState());
            _buttonState.Add(Button.QuickSave, new ButtonState());
            _buttonState.Add(Button.QuickLoad, new ButtonState());
            _buttonState.Add(Button.ToggleMinimap, new ButtonState());

            //buttonDown = new bool[3];
            //buttonPressed = new bool[3];
		}

		public bool IsButtonDown(Button button)
		{
            ButtonState buttonState;
            if (!_buttonState.TryGetValue(button, out buttonState))
                return false;

            return buttonState.IsButtonDown; //buttonDown[(int)button];
		}
		public void SetButtonDown(Button button, bool isDown)
		{
            ButtonState buttonState;
            if (!_buttonState.TryGetValue(button, out buttonState))
            {
                buttonState = new ButtonState();
                _buttonState.Add(button, buttonState);
            }
            buttonState.IsButtonDown = isDown;
		}

        public int GetButtonPressCount(Button button)
        {
            return _buttonState[button].ButtonPressCount;
        }

		public bool IsButtonPressed(Button button)
		{
            return _buttonState[button].IsButtonPressed;
		}
		public void SetButtonPressed(Button button, bool isPressed)
		{
            _buttonState[button].IsButtonPressed = isPressed;
            if (isPressed)
                _buttonState[button].ButtonPressCount++;
		}
        public void SetAllButtonsToDepressed()
        {
            foreach (var button in _buttonState)
            {
                button.Value.IsButtonPressed = false;
            }
            //for (int i=0;i<3;i++)
            //{
            //    buttonPressed[i] = false;
            //}
        }
	}

}