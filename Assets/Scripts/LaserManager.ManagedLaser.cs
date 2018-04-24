using UnityEngine;

public partial class LaserManager
{
    public class ManagedLaser
    {
        public Transform laser;
        private bool active;
        public bool inUse;

        public bool Active
        {
            get
            {
                active = laser.gameObject.activeSelf;
                return active;
            }
            set
            {
                laser.gameObject.SetActive(value);
                active = value;
            }
        }

        public ManagedLaser()
        {
            laser = Instantiate(laserTemplate);
            active = false;
        }
    }
}