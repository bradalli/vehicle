using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    public float verticalInput { get; set; }
    public float horizontalInput { get; set; }
    public Dictionary<string, bool> buttonPressStates { get; set; }

    public void AddButtonInput(string buttonName) => buttonPressStates.Add(buttonName, false);
    public void SetButtonValue( string buttonName, bool pressState)
    {
        if (buttonPressStates.ContainsKey(buttonName))
        {
            buttonPressStates[buttonName] = pressState;
        }

        else
        {
            AddButtonInput(buttonName);
        }
    }

    public bool GetButtonValue(string buttonName)
    {
        if (buttonPressStates.ContainsKey(buttonName))
        {
            return buttonPressStates[buttonName];
        }

        Debug.LogError(buttonName + "... does not exist in IControllable dictionary");
        return false;
    }
}
