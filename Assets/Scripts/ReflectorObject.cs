using UnityEngine;

public class ReflectorObject : LaserEmittingObject, ILaserableObject
{
    #region Methods

    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        throw new System.NotImplementedException();
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