using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Managers lasers
/// </summary>
public partial class LaserManager : MonoBehaviour
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

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    // Use this for initialization
    private void Start()
    {
        GameController = FindObjectOfType<GameController>();
        laserTemplate = Resources.Load<Transform>("Prefabs/Laser");
        if (!laserTemplate)
        {
            throw new MissingReferenceException("laserTemplate is not set");
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        laserList = new List<ManagedLaser>(FindObjectsOfType<LaserEmittingObject>().Length * 2);
        emitter = FindObjectOfType<EmitterObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameController.GameState == State.Running || GameController.GameState == State.MainMenu)
        {
            foreach (ManagedLaser laser in laserList)
            {
                laser.Active = false;
                laser.inUse = false;
            }
            emitter.Fire();
        }
    }

#endregion Unity Methods
}