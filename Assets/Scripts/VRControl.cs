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

        leftTrackedObject = LeftObject.GetComponent<SteamVR_TrackedObject>();
        rightTrackedObject = RightObject.GetComponent<SteamVR_TrackedObject>();

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
                Transform LNearest = GetNearestTurnable(LeftObject);
                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(LeftObject.transform.position, LNearest.position) <= activationDistance)
                {
                    LNearest.GetComponent<ITurnable>().TurnLeft();
                }

                Transform RNearest = GetNearestTurnable(RightObject);
                if (rightDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(RightObject.transform.position, RNearest.position) <= activationDistance)
                {
                    RNearest.GetComponent<ITurnable>().TurnRight();
                }

                if(leftDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) || rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
                {
                    GameController.PauseGame();
                }
            }
            else if(GameController.GameState == State.Paused)
            {
                if (leftDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) || rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
                {
                    GameController.ResumeGame();
                }
            }
            if(GameController.GameState == State.MainMenu || GameController.GameState == State.Paused)
            {
                if (rightDevice.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) && GetComponent<UIManager>().SelectedButton != null)
                {
                    GetComponent<UIManager>().SelectedButton.OnSubmit(new UnityEngine.EventSystems.BaseEventData(GetComponent<UIManager>().SelectedButton.transform.parent.parent.GetComponent<EventSystem>()));
                }
            }

            ////HACK: Debug bindings
            //if (leftDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu) || rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
            //{
            //    switch (GameController.GameState)
            //    {
            //        case State.MainMenu:
            //            {
            //                GameController.StartNextLevel();
            //                break;
            //            }
            //        case State.Running:
            //            {
            //                GameController.PauseGame();
            //                break;
            //            }
            //        case State.Paused:
            //            {
            //                GameController.ResumeGame();
            //                break;
            //            }
            //        case State.End:
            //            {
            //                GameController.ExitGame();
            //                break;
            //            }
            //    }

            //}
        }

    }

    #endregion Unity Methods
}