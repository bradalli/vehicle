using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Brad.Vehicle
{
    public class VehiclePhysics : MonoBehaviour
    {
        #region Public Variables
        [Header("Wheels")]
        public Transform flWheel;
        public Transform frWheel, blWheel, brWheel;
        public float fWheelWeight, bWheelWeight;
        public enum driveType { FrontWheelDrive, BackWheelDrive, FourWheelDrive }
        public driveType wheelDrive = driveType.FourWheelDrive;
        public LayerMask wheelRayMask;
        public float wheelRayDistance = 1;

        [Header("Steering")]
        public float wheelGrip;


        [Header("Suspension")]
        public float suspHeight = .5f;
        public float suspStrength = 10;
        public float suspDampening = 5;

        //[Header("Acceleration")]

        #endregion

        #region Private Variables
        // Components
        Rigidbody vehicleRb;
        Transform[] wheelsToSteerAndSuspend;
        Transform[] wheelsToAccelerate;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            // Cache components
            vehicleRb = GetComponent<Rigidbody>();
            wheelsToSteerAndSuspend = new Transform[] { flWheel, frWheel, blWheel, brWheel };

            // Cache wheels to accelerate based on wheel drive type
            switch (wheelDrive)
            {
                case driveType.BackWheelDrive:
                    wheelsToAccelerate = new Transform[] { blWheel, brWheel };
                    break;

                case driveType.FourWheelDrive:
                    wheelsToAccelerate = new Transform[] { flWheel, frWheel, blWheel, brWheel };
                    break;

                case driveType.FrontWheelDrive:
                    wheelsToAccelerate = new Transform[] { flWheel, frWheel };
                    break;

                default:
                    wheelsToAccelerate = new Transform[] { flWheel, frWheel, blWheel, brWheel };
                    break;
            }
        }

        private void Update()
        {
            // Add force to each wheel in wheelsToSteerAndSuspend
            for(int i = 0; i < wheelsToSteerAndSuspend.Length; i++)
            {
                if (Physics.Raycast(wheelsToSteerAndSuspend[i].position, -wheelsToSteerAndSuspend[i].up, out RaycastHit hit, wheelRayDistance, wheelRayMask))
                {
                    Debug.DrawLine(wheelsToSteerAndSuspend[i].position, hit.point, Color.red);

                    Vector3 wheelForce = SuspensionForce(wheelsToSteerAndSuspend[i], hit) + SteeringForce(wheelsToSteerAndSuspend[i]);
                    vehicleRb.AddForceAtPosition(wheelForce, wheelsToSteerAndSuspend[i].position, ForceMode.Force);
                }
            }

            // Add force to each wheel in wheelsToAccelerate
            for (int i = 0; i < wheelsToAccelerate.Length; i++)
            {
                if (Physics.Raycast(wheelsToAccelerate[i].position, -wheelsToAccelerate[i].up, wheelRayDistance, wheelRayMask))
                {

                }
            }
        }

        #endregion

        #region Custom Methods

        // Calculate suspension force for this wheel
        Vector3 SuspensionForce(Transform wheelTransform, RaycastHit wheelRayHit)
        {
            // Direction for the suspension force
            Vector3 suspDirection = wheelTransform.up;

            // World velocity of the wheel
            Vector3 wheelVelocity = vehicleRb.GetPointVelocity(wheelTransform.position);

            // Get offset
            float offset = suspHeight - wheelRayHit.distance;

            // Get target velcocity
            float targetVelocity = Vector3.Dot(suspDirection, wheelVelocity);

            // Caculate suspension force
            float suspensionForce = (offset * suspStrength) - (targetVelocity * suspDampening);

            // Return the final force
            return suspDirection * suspensionForce;
        }

        // Calculate steering force for this wheel
        Vector3 SteeringForce(Transform wheelTransform)
        {
            return Vector3.zero;
        }

        // Calculate acceleration force for this wheel
        Vector3 AccelerationForce(Transform wheelTransform)
        {
            return Vector3.zero;
        }

        #endregion
    }
}

