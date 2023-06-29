using Brad.Vehicle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateSpeedUI : MonoBehaviour
{
    [SerializeField] VehiclePhysics vehicle;
    [SerializeField] TextMeshProUGUI text;
    public enum SpeedUnits { MetersPerSecond, MilesPerHour, KilometersPerHour }
    public SpeedUnits targetUnit;

    void FixedUpdate()
    {
        switch (targetUnit)
        {
            case SpeedUnits.MetersPerSecond:
                text.text = (vehicle.currentSpeed).ToString("0m/s");
                break;

            case SpeedUnits.MilesPerHour:
                text.text = (vehicle.currentSpeed * 2.23694f).ToString("0mph");
                break;

            case SpeedUnits.KilometersPerHour:
                text.text = (vehicle.currentSpeed * 3.6f).ToString("0mph");
                break;

            default:
                text.text = (vehicle.currentSpeed).ToString("0m/s");
                break;
        }
    }
}
