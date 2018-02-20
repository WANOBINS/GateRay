using UnityEngine;

public class ReflectorObject : LaserEmittingObject, ILaserableObject, ITurnable
{
    
    #region Methods

    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        throw new System.NotImplementedException();
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