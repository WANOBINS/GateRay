﻿using System;
using UnityEngine;

/// <summary>
/// The laser emitter
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EmitterObject : LaserEmittingObject, ITurnable
{
    #region Variables

    //private bool laserActive = false;

    #endregion Variables

    #region Methods
    /// <summary>
    /// Called to fire the initial laser
    /// </summary>
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
}