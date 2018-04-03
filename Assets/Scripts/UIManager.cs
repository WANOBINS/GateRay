using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    GameController GameController;
    VRControl VRControl;
    Material LaserMat;

    GameObject LController;
    GameObject RController;
    GameObject LLaserDummy;
    GameObject RLaserDummy;
    LineRenderer LLaser;
    LineRenderer RLaser;
    float DefaultLaserLength = 100f;

    Vector3 LaserRot = new Vector3(40, 0, 0);
    const float LaserLRAdjust = -.0075f;
    const float LaserUDAdjust = -0.02f;
    const float LaserFRAdjust = -0.05f;
    Vector3 RLaserPos = new Vector3(LaserLRAdjust, LaserUDAdjust, LaserFRAdjust);
    Vector3 LLaserPos = new Vector3(-LaserLRAdjust, LaserUDAdjust, LaserFRAdjust);

    public Button SelectedButton { get; private set; }

    private void ConfigureLaser(LineRenderer Laser)
    {
        Laser.materials = new Material[] { LaserMat };
        Laser.startColor = Color.red;
        Laser.endColor = Color.red;
        Laser.widthMultiplier = .01f;
        Laser.sortingLayerName = "Pointers";
        Laser.enabled = false;
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        LaserMat = Resources.Load<Material>("Materials/Laser Pointer");
    }

	// Update is called once per frame
	void Update () { 
		if(GameController.GameState == State.MainMenu || GameController.GameState == State.Paused)
        {
            //LLaser.enabled = true;
            RLaser.enabled = true;

            //Ray LRay = new Ray(LLaserDummy.transform.position, LLaserDummy.transform.forward);
            //RaycastHit LHit;

            Ray RRay = new Ray(RLaserDummy.transform.position, RLaserDummy.transform.forward);
            RaycastHit RHit;

            //LLaser.SetPosition(0, LLaserDummy.transform.position);
            RLaser.SetPosition(0, RLaserDummy.transform.position);

            //if (Physics.Raycast(LRay,out LHit, Mathf.Infinity,LayerMask.GetMask("UI")))
            //{
            //    LLaser.SetPosition(1, LHit.point);
            //    Debug.Log("LPointer Hit: " + LHit.transform.name);
            //}
            //else
            //{
            //    LLaser.SetPosition(1, LLaser.transform.forward + (LRay.direction * DefaultLaserLength));
            //}

            if(Physics.Raycast(RRay,out RHit, Mathf.Infinity, LayerMask.GetMask("UI")))
            {
                RLaser.SetPosition(1, RHit.point);
                RHit.transform.GetComponent<Button>().Select();
                SelectedButton = RHit.transform.GetComponent<Button>();
                Debug.Log("RPointer Hit: " + RHit.transform.name);
            }
            else
            {
                SelectedButton = null;
                RLaser.SetPosition(1, RLaser.transform.forward + (RRay.direction * DefaultLaserLength));
            }
        }
        else
        {
            SelectedButton = null;
            //LLaser.enabled = false;
            RLaser.enabled = false;
        }
	}

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameController = GetComponent<GameController>();
        VRControl = GetComponent<VRControl>();

        LController = VRControl.LeftObject;
        RController = VRControl.RightObject;

        LLaserDummy = new GameObject
        {
            name = "Left Laser Dummy"
        };
        LLaserDummy.transform.parent = LController.transform;
        LLaserDummy.transform.localPosition = LLaserPos;
        LLaser = LLaserDummy.AddComponent<LineRenderer>();
        ConfigureLaser(LLaser);
        LLaser.transform.localEulerAngles = LaserRot;
        LLaser.enabled = true;

        RLaserDummy = new GameObject
        {
            name = "Right Laser Dummy"
        };
        RLaserDummy.transform.parent = RController.transform;
        RLaserDummy.transform.localPosition = RLaserPos;
        RLaser = RLaserDummy.AddComponent<LineRenderer>();
        ConfigureLaser(RLaser);
        RLaser.transform.localEulerAngles = LaserRot;
        RLaser.enabled = true;
    }
}
