using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserableObject : MonoBehaviour {

    public bool IsEndGoal;
    public bool DestroyOnLasered;
    public bool Lasered;
    public LaserEmittingObject2 LaseringObject;
    protected GameController Controller;

    // Use this for initialization
    void Start () {
        Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called by a LaserEmittingObject when it hits a LaserableObject
    /// </summary>
    /// <param name="_LaseringObject"></param>
    public virtual void OnLasered(GameObject _LaseringObject)
    {
        LaseringObject = _LaseringObject.GetComponent<LaserEmittingObject2>();
        if (DestroyOnLasered)
        {
            Destroy(gameObject);
        }
        if (IsEndGoal)
        {
            Controller.EndLevel();
        }
    }
}
