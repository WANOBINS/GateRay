using UnityEngine;

public class LaserEmittingObject : MonoBehaviour, ITurnable
{
    protected LaserManager LaserManager;
    protected AudioSource AudioSource;
    protected GameController GameController;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void TurnLeft()
    {
        AudioSource.PlayOneShot(GameController.TurnSound);
        transform.Rotate(transform.up, -(float)GameController.MirrorTurnIncrement);
    }

    public void TurnRight()
    {
        AudioSource.PlayOneShot(GameController.TurnSound);
        transform.Rotate(transform.up, (float)GameController.MirrorTurnIncrement);
    }

    // Awake is called when the script instance is being loaded
    protected void Awake()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        LaserManager = controller.GetComponent<LaserManager>();
        GameController = controller.GetComponent<GameController>();
        AudioSource = GetComponent<AudioSource>();
    }


    // Use this for initialization
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        if(AudioSource == null)
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}