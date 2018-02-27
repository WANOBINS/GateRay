using UnityEngine;

/// <summary>
/// The target of the level
/// </summary>
public class LaserTarget : MonoBehaviour,ILaserableObject
{
    GameController GameController;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        GameController = FindObjectOfType<GameController>();
    }


    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        transform.Find("RecieverOrb").GetComponent<Renderer>().material = GameController.WinMat;
        GameController.FinishLevel();
    }
}