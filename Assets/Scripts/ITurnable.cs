using UnityEngine;

/// <summary>
/// An object that can be turned by VR Controls
/// </summary>
public interface ITurnable
{
    void TurnLeft();

    void TurnRight();

    GameObject GetGameObject();
}