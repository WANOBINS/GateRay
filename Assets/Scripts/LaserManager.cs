using System;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public class ManagedLaser
    {
        private Transform laser;
        private bool active;
    }

    public Transform laserTemplate;
    private List<ManagedLaser> laserList;

    // Use this for initialization
    private void Start()
    {
        laserTemplate = Resources.Load<Transform>("/Prefabs/Laser");
        if (!laserTemplate)
        {
            throw new NullReferenceException("laserTemplate is not set");
        }
        laserList = new List<ManagedLaser>(FindObjectsOfType<LaserEmittingObject>().Length);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}