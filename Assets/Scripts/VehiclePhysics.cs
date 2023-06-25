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
        public float wheelWeight;
        public enum driveType { FrontWheelDrive, BackWheelDrive, FourWheelDrive }
        public driveType wheelDrive = driveType.FourWheelDrive;
        public LayerMask wheelRayMask;
        public float wheelRayDistance = 1;

        [Header("Steering")]
        public AnimationCurve fWheelGripCurve;
        public AnimationCurve bWheelGripCurve;

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
                    // Cache target wheel
                    Transform targetWheel = wheelsToSteerAndSuspend[i];

                    // Apply force
                    Vector3 wheelForce = SuspensionForce(targetWheel, hit) + SteeringForce(targetWheel);
                    vehicleRb.AddForceAtPosition(wheelForce, targetWheel.position, ForceMode.Force);

                    #region Visualisation
                    // Force visual
                    Debug.DrawLine(targetWheel.position, targetWheel.position + wheelForce, Color.cyan);
                    // Local transform visual
                    VisualiseLocalTransform(targetWheel);
                    #endregion
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

            #region Visualisation
            // Raycast visualisation
            Debug.DrawLine(wheelTransform.position, wheelRayHit.collider ? wheelRayHit.point : wheelTransform.position + wheelTransform.up * -suspHeight, Color.yellow);
            #endregion

            // Return the final force
            return suspDirection * suspensionForce;
        }

        // Calculate steering force for this wheel
        Vector3 SteeringForce(Transform wheelTransform)
        {
            // Direction for the steering force
            Vector3 steerDirection = wheelTransform.right;

            // World velocity of the wheel
            Vector3 wheelVelocity = vehicleRb.GetPointVelocity(wheelTransform.position);

            // Steering velocity
            float steerVelocity = Vector3.Dot(steerDirection, wheelVelocity);

            // Get wheelGripAmount depending on if its a front or back wheel
            float wheelGripAmount = wheelTransform.name.Contains('F') ? fWheelGripCurve.Evaluate(steerVelocity) : bWheelGripCurve.Evaluate(steerVelocity);

            // Target velocity change amount
            float targetVelocityChange = -steerVelocity * wheelGripAmount;

            // Target acceleration
            float targetAcceleration = targetVelocityChange / Time.fixedDeltaTime;

            // Return the final force
            return steerDirection * wheelWeight * targetAcceleration;
        }

        // Calculate acceleration force for this wheel
        Vector3 AccelerationForce(Transform wheelTransform)
        {
            // Return the final force
            return Vector3.zero;
        }

        void VisualiseLocalTransform(Transform targetTransform)
        {
            // Right vector
            Debug.DrawLine(targetTransform.position, targetTransform.position + targetTransform.right, Color.red);

            // Up vector
            Debug.DrawLine(targetTransform.position, targetTransform.position + targetTransform.up, Color.green);

            // Forward vector
            Debug.DrawLine(targetTransform.position, targetTransform.position + targetTransform.forward, Color.blue);
        }

        #endregion
    }
}

