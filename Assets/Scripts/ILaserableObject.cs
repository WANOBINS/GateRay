using UnityEngine;

/// <summary>
/// An object that reacts when it by a laser
/// </summary>
public interface ILaserableObject
{
    void LaserHit(LaserEmittingObject other, RaycastHit hit);
}