using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managers lasers
/// </summary>
public class LaserManager : MonoBehaviour
{
    #region Variables

    public static Transform laserTemplate;

    private EmitterObject emitter;
    private List<ManagedLaser> laserList;
    private GameController GameController;

    #endregion Variables

    #region Methods
    /// <summary>
    /// Gets an inactive laser from the LaserManager
    /// </summary>
    /// <returns></returns>
    public ManagedLaser RequestLaser()
    {
        for(int i = 0; i < laserList.Count; i++)
        {
            if (!laserList[i].inUse)
            {
                laserList[i].inUse = true;
                return laserList[i];
            }
        }
        ManagedLaser output = new ManagedLaser
        {
            inUse = true
        };
        laserList.Add(output);
        return output;
    }
    #endregion Methods

    #region Unity Methods

    // Use this for initialization
    private void Start()
    {
        GameController.FindObjectOfType<GameController>();
        laserTemplate = Resources.Load<Transform>("Prefabs/Laser");
        if (!laserTemplate)
        {
            throw new NullReferenceException("laserTemplate is not set");
        }
        laserList = new List<ManagedLaser>(FindObjectsOfType<LaserEmittingObject>().Length * 2);
        emitter = FindObjectOfType<EmitterObject>();
        GameController.LasersReady();
    }

    // Update is called once per frame
    private void Update()
    {
        foreach(ManagedLaser laser in laserList)
        {
            laser.Active = false;
            laser.inUse = false;
        }
        emitter.Fire();
    }

    #endregion Unity Methods

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