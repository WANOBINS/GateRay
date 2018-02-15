using System;
using UnityEngine;
using Valve.VR;

public class VRControl : MonoBehaviour
{
    #region Variables

    public float activationDistance = 1f;

    [SerializeField]
    private GameObject leftObject;
    [SerializeField]
    private GameObject rightObject;

    private SteamVR_TrackedObject leftTrackedObject;
    private SteamVR_TrackedObject rightTrackedObject;

    private SteamVR_Controller.Device leftDevice;
    private SteamVR_Controller.Device rightDevice;

    #endregion Variables

    #region Methods
    private GameObject GetNearestTurnable()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Unity Methods

    // Use this for initialization
    private void Start()
    {
        GameObject CamRig = GameObject.Find("[CameraRig]");
        SteamVR_ControllerManager ControllerManager = CamRig.GetComponent<SteamVR_ControllerManager>();
        leftObject = ControllerManager.left;
        rightObject = ControllerManager.right;

        leftTrackedObject = leftObject.GetComponent<SteamVR_TrackedObject>();
        rightTrackedObject = rightObject.GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        leftDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);
        rightDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);

        if (leftDevice.GetPressDown(EVRButtonId.k_EButton_A) && Vector3.Distance(leftObject.transform.position,GetNearestTurnable().transform.position) <= activationDistance)
        {

        }

    }

    #endregion Unity Methods
}