using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

/// <summary>
/// Gets button presses from VR controllers
/// </summary>
public class VRControl : MonoBehaviour
{
    #region Variables

    public float activationDistance = 1f;

    private GameController GameController;

    [SerializeField]
    private GameObject leftObject;
    [SerializeField]
    private GameObject rightObject;

    private SteamVR_TrackedObject leftTrackedObject;
    private SteamVR_TrackedObject rightTrackedObject;

    private SteamVR_Controller.Device leftDevice;
    private SteamVR_Controller.Device rightDevice;

    private LineRenderer LLine;
    private LineRenderer RLine;

    //Note: Turnables System only works for stationary turnables
    private List<ITurnable> Turnables;

    #endregion Variables

    #region Methods
    private Transform GetNearestTurnable(GameObject controllerObject)
    {
        GameObject CurrentClosest = null;
        foreach(ITurnable turnable in Turnables)
        {
            if(CurrentClosest == null || Vector3.Distance(controllerObject.transform.position, turnable.GetGameObject().transform.position) < Vector3.Distance(controllerObject.transform.position,CurrentClosest.transform.position))
            {
                CurrentClosest = turnable.GetGameObject();
            }
        }
        return CurrentClosest.transform;
    }

    #endregion

    #region Unity Methods

    // Use this for initialization
    private void Start()
    {
        Debug.Log("Loading...");
        GameObject CamRig = GameObject.Find("[CameraRig]");
        SteamVR_ControllerManager ControllerManager = CamRig.GetComponent<SteamVR_ControllerManager>();
        leftObject = ControllerManager.left;
        rightObject = ControllerManager.right;
        GameController = FindObjectOfType<GameController>();

        LLine = leftObject.AddComponent<LineRenderer>();
        RLine = rightObject.AddComponent<LineRenderer>();



        leftTrackedObject = leftObject.GetComponent<SteamVR_TrackedObject>();
        rightTrackedObject = rightObject.GetComponent<SteamVR_TrackedObject>();

        Turnables = new List<ITurnable>();

        foreach(GameObject go in FindObjectsOfType<GameObject>())
        {
            if(go.GetComponent<ITurnable>() != null)
            {
                Debug.Log("Adding " + go.name + " to list of turnables");
                Turnables.Add(go.GetComponent<ITurnable>());
            }
        }
        Debug.Log("Loaded!");
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameController.GameState != State.Invalid && GameController.GameState != State.Loading) //Not loading or errored
        {
            //Fetch device states
            try
            {
                leftDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);
                rightDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

            //Turn controls only work when the game is in the Running phase
            if (GameController.GameState == State.Running)
            {
                Transform LNearest = GetNearestTurnable(leftObject);
                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(leftObject.transform.position, LNearest.position) <= activationDistance)
                {
                    LNearest.GetComponent<ITurnable>().TurnLeft();
                }

                Transform RNearest = GetNearestTurnable(rightObject);
                if (rightDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(rightObject.transform.position, RNearest.position) <= activationDistance)
                {
                    RNearest.GetComponent<ITurnable>().TurnRight();
                }
            }
            else if(GameController.GameState == State.MainMenu)
            {

            }

            //HACK: Debug bindings
            if (leftDevice.GetPressDown(EVRButtonId.k_EButton_System))
            {
                switch (GameController.GameState)
                {
                    case State.MainMenu:
                        {
                            GameController.StartNextLevel();
                            break;
                        }
                    case State.Running:
                        {
                            GameController.PauseGame();
                            break;
                        }
                    case State.Paused:
                        {
                            GameController.ResumeGame();
                            break;
                        }
                    case State.End:
                        {
                            GameController.ExitGame();
                            break;
                        }
                }

            }
        }

    }

    #endregion Unity Methods
}