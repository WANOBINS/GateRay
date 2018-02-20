using UnityEngine;

public class LaserEmittingObject : MonoBehaviour
{
    protected LaserManager LaserManager;

    // Awake is called when the script instance is being loaded
    protected void Awake()
    {
        LaserManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LaserManager>();
    }


    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}