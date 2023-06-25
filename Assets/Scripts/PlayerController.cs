using Brad.Vehicle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    IControllable targetToControl;

    public UnityEvent startBrake, stopBrake, flipVehicle;

    private void Start()
    {
        targetToControl = gameObject.GetComponent<IControllable>();
    }

    void Update()
    {
        targetToControl.verticalInput = Input.GetAxis("Vertical");
        targetToControl.horizontalInput = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Jump"))
            startBrake.Invoke();

        if (Input.GetButtonUp("Jump"))
            stopBrake.Invoke();

        if (Input.GetButtonDown("Fire1"))
            flipVehicle.Invoke();
    }
}
