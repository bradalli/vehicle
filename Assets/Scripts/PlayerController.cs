using Brad.Vehicle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IControllable targetToControl;

    private void Start()
    {
        targetToControl = gameObject.GetComponent<IControllable>();
        targetToControl.AddButtonInput("Jump");
    }

    void Update()
    {
        targetToControl.verticalInput = Input.GetAxis("Vertical");
        targetToControl.horizontalInput = Input.GetAxis("Horizontal");
        targetToControl.SetButtonValue("Jump", Input.GetButton("Jump"));
    }
}
