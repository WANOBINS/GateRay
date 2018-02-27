using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameController
/// </summary>
public class GameController : MonoBehaviour
{
    #region Variables
    public float MirrorTurnIncrement = 22.5f;
    public float NextLevelDelay = 5f;
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

    #endregion Variables

    #region Methods

    public void FinishLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1)
        {
            GameState = State.End;
            ShowEndMenu();
        }
        else
        {
            StartCoroutine(LoadNextLevel());
            GameState = State.Loading;
        }
    }

    private void ShowEndMenu()
    {
        throw new NotImplementedException();
    }

    private IEnumerator LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        else if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount - 1)
        {
            yield return new WaitForSeconds(NextLevelDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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

    // Use this for initialization
    private void Start()
    {
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