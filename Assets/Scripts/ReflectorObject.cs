using UnityEngine;

/// <summary>
/// A mirror
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ReflectorObject : LaserEmittingObject, ILaserableObject, ITurnable
{
    #region Methods

    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        Vector3 OutDir = Vector3.Reflect((transform.position - other.transform.position).normalized, transform.forward);
        LaserManager.ManagedLaser Laser = LaserManager.RequestLaser();
        Laser.laser.position = hit.point + LaserOffest;
        Laser.laser.rotation = Quaternion.LookRotation(OutDir);
        Laser.Active = true;
        Ray beam = new Ray(hit.point, OutDir);
        if (Physics.Raycast(beam, out hit,Mathf.Infinity))
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
            Laser.laser.localScale = new Vector3(1, 1, 100);
            Debug.Log("No Hit");
        }
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