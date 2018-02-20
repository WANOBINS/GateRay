using UnityEngine;

public class ReflectorObject : LaserEmittingObject, ILaserableObject, ITurnable
{
    #region Methods

    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        Vector3 OutDir = Vector3.Reflect((other.transform.position - transform.position).normalized, transform.forward);
        LaserManager.ManagedLaser Laser = LaserManager.RequestLaser();
        Laser.laser.position = hit.point;
        Laser.laser.rotation = Quaternion.LookRotation(OutDir);
        Laser.Active = true;
        Ray beam = new Ray(hit.point, OutDir);
        if (Physics.Raycast(beam, out hit))
        {
            Laser.laser.localScale = new Vector3(1, 1, Vector3.Distance(beam.origin, hit.point));
            ILaserableObject laserableObject = hit.collider.gameObject.GetComponent<ILaserableObject>();
            if (laserableObject != null)
            {
                laserableObject.LaserHit(this, hit);
            }
        }
        else
        {
            Laser.laser.localScale = new Vector3(1, 1, Mathf.Infinity);
        }
    }

    public void TurnLeft()
    {
        transform.Rotate(transform.up, -12.5f);
    }

    public void TurnRight()
    {
        transform.Rotate(transform.up, 12.5f);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    #endregion Methods

    #region Unity Methods

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion Unity Methods
}