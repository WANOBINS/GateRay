using UnityEngine;

public class LaserTarget : ILaserableObject
{
    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        Object.FindObjectOfType<GameController>().FinishLevel();
    }
}