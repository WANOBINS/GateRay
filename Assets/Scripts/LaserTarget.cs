using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The target of the level
/// </summary>
public class LaserTarget : MonoBehaviour,ILaserableObject
{
    GameController GameController;
    AudioClip winSound;
    public float volume = .5f;
    private bool done = false;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        GameController = FindObjectOfType<GameController>();
        winSound = Resources.Load<AudioClip>("Audio/YouWinSound");
    }


    public void LaserHit(LaserEmittingObject other, RaycastHit hit)
    {
        transform.Find("Sphere").GetComponent<Renderer>().material = GameController.WinMat;

        if (!done)
        {
            done = true;
            GetComponent<AudioSource>().PlayOneShot(winSound, volume);
        }

        if (SceneManager.GetActiveScene().name != "Test Scene")
        {
            GameController.FinishLevel();
        }
    }
}