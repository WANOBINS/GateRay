using UnityEngine;

public interface ILaserableObject
{
    void LaserHit(LaserEmittingObject other, RaycastHit hit);
}