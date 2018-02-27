using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameController
/// </summary>
public class GameController : MonoBehaviour
{
    #region Variables
    public double MirrorTurnIncrement = 22.5;
    public double NextLevelDelay = 5;
    internal bool isGameReady;
    private AudioClip _BGMusic;
    private AudioClip _TurnSound;
    private AudioClip _WinSound;
    private GameObject MenuSuiteTemplate;
    private Material _WinMat;
    private GameObject MenuSuite;
    private GameObject PauseMenu;
    private State _GameState;

    private GameObject CameraObject;
    private GameObject EarCameraObject;

    private Camera Camera;
    private AudioListener Ears;

    #region ReadyUp
    bool VRControl = false;
    bool Lasers = false;
    #endregion ReadyUp

    private AudioSource BGMusicSource;

    public AudioClip BGMusic
    {
        get
        {
            return _BGMusic;
        }
        private set
        {
            _BGMusic = value;
        }
    }

    public AudioClip TurnSound
    {
        get
        {
            return _TurnSound;
        }
        private set
        {
            _TurnSound = value;
        }
    }

    public AudioClip WinSound
    {
        get
        {
            return _WinSound;
        }
        private set
        {
            _WinSound = value;
        }
    }

    public State GameState
    {
        get
        {
            return _GameState;
        }

        private set
        {
            _GameState = value;
        }
    }

    public Material WinMat
    {
        get
        {
            return _WinMat;
        }

        private set
        {
            _WinMat = value;
        }
    }

    internal void LasersReady()
    {
        Lasers = true;
    }

    public bool LevelReady { get; private set; }

    #endregion Variables

    #region Methods

    public void FinishLevel()
    {
        throw new NotImplementedException();
    }

    #endregion Methods

    #region Unity Methods

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        GameState = State.Loading;
        BGMusic = Resources.Load<AudioClip>("Audio/GRBackgroundMusic");
        TurnSound = Resources.Load<AudioClip>("Audio/RotateSound");
        WinSound = Resources.Load<AudioClip>("Audio/YouWinSound");
        MenuSuiteTemplate = Resources.Load<GameObject>("Prefabs/MenuSuite");
        WinMat = Resources.Load<Material>("Materials/RecieverOn");

        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        EarCameraObject = GameObject.Find("Camera (ears)");

        Camera = CameraObject.GetComponent<Camera>();
        Ears = EarCameraObject.GetComponent<AudioListener>();
    }

    internal void VRControlReady()
    {
        VRControl = true;
    }

    // Use this for initialization
    private void Start()
    {
        while (!LevelReady)
        {
            if(VRControl && Lasers)
            {
                LevelReady = true;
            }
        }
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            GameState = State.MainMenu;
        }
        else
        {
            GameState = State.Running;
        }
        BGMusicSource = EarCameraObject.AddComponent<AudioSource>();
        BGMusicSource.playOnAwake = false;
        BGMusicSource.loop = true;
        BGMusicSource.clip = BGMusic;
        BGMusicSource.Play();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion Unity Methods

    public enum State
    {
        Invalid, //Errored State
        Loading, //Loading
        MainMenu, //Main Menu
        Running, //Running
        Paused, //Paused
        End //End Screen
    }

    internal void PauseGame()
    {
        throw new NotImplementedException();
    }

    internal void ResumeGame()
    {
        throw new NotImplementedException();
    }

    internal void StartNextLevel()
    {
        throw new NotImplementedException();
    }

    internal void ExitGame()
    {
        throw new NotImplementedException();
    }
}