using UnityEngine;

namespace oldScripts
{
    public class ReflectorObject2 : LaserEmittingObject2
    {
        #region Variables

        public Vector3 ReflectionCenter;
        public Quaternion IncomingLaserDirection;
        public Vector3 IncomingLaserDirectionV3;
        public Quaternion OutgoingLaserDirection;
        public Vector3 OutgoingLaserDirectionV3;
        public float HalfAngle;
        public float FullAngle;
        public float MaxAngle = Mathf.Infinity;
        private Vector3 OutgoingLaserDirectionEuler;

        #endregion Variables

        #region Methods

        public override void OnLasered(GameObject LaseringObject)
        {
            base.OnLasered(LaseringObject);
            Lasered = true;
            Quaternion TempQuat = gameObject.transform.rotation;
            Vector3 TempV3 = TempQuat.eulerAngles;
            TempV3.y = TempV3.y + AngleTweak;
            TempQuat.eulerAngles = TempV3;
            ReflectionCenter = TempQuat * Vector3.forward;
            if (LaseringObject.GetComponent<LaserEmittingObject2>().LaseredObject != gameObject.GetComponent<LaserableObject>())
            {
                Lasered = false;
            }
            if (LaseredObject != null)
            {
                if (LaseredObject.GetComponent<LaserableObject>() != null)
                {
                    LaseredObject.GetComponent<LaserableObject>().Lasered = false;
                }
            }
            if (Lasered)
            {
                IncomingLaserDirection = Quaternion.LookRotation(gameObject.transform.position - LaseringObject.transform.position); //Quat
                IncomingLaserDirectionV3 = IncomingLaserDirection * Vector3.forward; //Dir
                FullAngle = -(Vector3.Angle(IncomingLaserDirectionV3, OutgoingLaserDirectionV3) - 180);
                HalfAngle = FullAngle / 2;
                OutgoingLaserDirectionEuler = new Vector3(IncomingLaserDirectionV3.x, IncomingLaserDirectionV3.y + FullAngle, IncomingLaserDirectionV3.z);
                OutgoingLaserDirection.eulerAngles = OutgoingLaserDirectionEuler;
                OutgoingLaserDirectionV3 = Vector3.Reflect(IncomingLaserDirectionV3, ReflectionCenter);
                Debug.DrawRay(gameObject.transform.position, ReflectionCenter, Color.blue);
                Debug.DrawRay(gameObject.transform.position, IncomingLaserDirectionV3, Color.black);
                Debug.DrawRay(gameObject.transform.position, OutgoingLaserDirectionV3, Color.red);
                if (Mathf.Abs(FullAngle) < MaxAngle)
                {
                    SendLaser(OutgoingLaserDirectionV3);
                }
            }
            else
            {
                HideLaser();
            }
        }

        #endregion Methods

        #region Unity Methods

        // Use this for initialization
        private void Start()
        {
            CommonStart();
        }

        private void Update()
        {
            HideLaser();
        }

        #endregion Unity Methods
    }
}