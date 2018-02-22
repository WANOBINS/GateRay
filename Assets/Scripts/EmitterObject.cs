using System;
using UnityEngine;

public class EmitterObject : LaserEmittingObject, ITurnable
{
    #region Variables

    //private bool laserActive = false;
    public Vector3 LaserOffest = new Vector3(0, 1.1316f, 0);

    #endregion Variables

    #region Methods

    internal void Fire()
    {
        LaserManager.ManagedLaser Laser = LaserManager.RequestLaser();
        Ray beam = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Laser.laser.position = beam.origin + LaserOffest;
        Laser.laser.rotation = Quaternion.LookRotation(beam.direction);
        Laser.Active = true;
        if(Physics.Raycast(beam,out hit,Mathf.Infinity))
        {
            Debug.Log("I hit: " + hit.collider.gameObject.name);
            Laser.laser.localScale = new Vector3(1,1,Vector3.Distance(beam.origin, hit.point));
            ILaserableObject laserableObject = hit.collider.gameObject.GetComponent<ILaserableObject>();
            if (laserableObject != null)
            {
                laserableObject.LaserHit(this, hit);
            }
        }
        else
        {
            Laser.laser.localScale = new Vector3(1, 1, 100);
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