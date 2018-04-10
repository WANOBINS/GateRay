using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Valve.VR;

/// <summary>
/// Gets button presses from VR controllers
/// </summary>
public class VRControl : MonoBehaviour
{
    #region Variables

    public float activationDistance = 2f;

    private GameController GameController;
    private Transform Floor;
    private List<Vector3> ArcPoints = new List<Vector3>();

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

    private TeleportState teleportState = VRControl.TeleportState.Inactive;

    private enum TeleportState
    {
        Inactive,
        LTargeting,
        RTargeting,
        Deactivating
    }

    //Note: Turnables System only works for stationary turnables
    private List<ITurnable> Turnables;

    public GameObject LeftObject
    {
        get
        {
            return leftObject;
        }

        private set
        {
            leftObject = value;
        }
    }

    public GameObject RightObject
    {
        get
        {
            return rightObject;
        }

        private set
        {
            rightObject = value;
        }
    }

    #endregion Variables

    #region Methods
    private Transform GetNearestTurnable(GameObject controllerObject)
    {
        GameObject CurrentClosest = null;
        foreach (ITurnable turnable in Turnables)
        {
            if (CurrentClosest == null || Vector3.Distance(controllerObject.transform.position, turnable.GetGameObject().transform.position) < Vector3.Distance(controllerObject.transform.position, CurrentClosest.transform.position))
            {
                CurrentClosest = turnable.GetGameObject();
            }
        }
        return CurrentClosest.transform;
    }

    #endregion

    #region Unity Methods

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loading...");
        GameObject CamRig = GameObject.Find("[CameraRig]");
        SteamVR_ControllerManager ControllerManager = CamRig.GetComponent<SteamVR_ControllerManager>();
        LeftObject = ControllerManager.left;
        RightObject = ControllerManager.right;
        GameController = FindObjectOfType<GameController>();
        Floor = GameObject.Find("Floor").transform;

        leftTrackedObject = LeftObject.GetComponent<SteamVR_TrackedObject>();
        rightTrackedObject = RightObject.GetComponent<SteamVR_TrackedObject>();

        Turnables = new List<ITurnable>();

        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (go.GetComponent<ITurnable>() != null)
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

            //Turn controls only work when the game is in the Running and Main Menu phases
            if (GameController.GameState == State.Running || GameController.GameState == State.MainMenu)
            {
                Debug.Log("Normal Input Active");
                Transform LNearest = GetNearestTurnable(LeftObject);
                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(LeftObject.transform.position, LNearest.position) <= activationDistance)
                {
                    Debug.Log("Attempting to turn " + LNearest + " left");
                    LNearest.GetComponent<ITurnable>().TurnLeft();
                }

                Transform RNearest = GetNearestTurnable(RightObject);
                if (rightDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(RightObject.transform.position, RNearest.position) <= activationDistance)
                {
                    Debug.Log("Attempting to turn " + RNearest + " right");
                    RNearest.GetComponent<ITurnable>().TurnRight();
                }

                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) || rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
                {
                    GameController.PauseGame();
                }
            }
            else if (GameController.GameState == State.Paused)
            {
                Debug.Log("Menu Input Active");
                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) || rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
                {
                    GameController.ResumeGame();
                }
                if (rightDevice.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) && GetComponent<UIManager>().SelectedButton != null)
                {
                    GetComponent<UIManager>().SelectedButton.OnSubmit(new BaseEventData(GetComponent<UIManager>().SelectedButton.transform.parent.parent.GetComponent<EventSystem>()));
                }
            }

            //If TP system isn't resetting
            if (teleportState != TeleportState.Deactivating)
            {
                //If one is pushed and the other isn't
                if (leftDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) ^ rightDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    //If left is pushed
                    if (leftDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        teleportState = TeleportState.LTargeting;
                        //Show Arc L
                    }
                    //If button is released and we're targeting left
                    if (leftDevice.GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad) && teleportState == TeleportState.LTargeting)
                    {
                        //Teleport L
                    }

                    //If right is pushed
                    if (rightDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        teleportState = TeleportState.RTargeting;
                        //Show Arc R
                    }
                    //If the button is released and we're targeting right
                    if (rightDevice.GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad) && teleportState == TeleportState.RTargeting)
                    {
                        //Teleport R
                    }
                }
                //If both are pushed
                else if(leftDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) && rightDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    //Set to deactivate
                    teleportState = TeleportState.Deactivating;
                }
            }
            if(teleportState == TeleportState.Deactivating)
            {
                if(!leftDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad) && !rightDevice.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    teleportState = TeleportState.Inactive;
                }
            }
        }
    }

    #endregion Unity Methods
}