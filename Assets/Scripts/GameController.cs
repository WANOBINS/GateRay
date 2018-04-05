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
    [SerializeField]
    private State _GameState = State.Invalid;

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

    internal void LoadLevel(int sceneIndex)
    {
        GameState = State.Loading;
        SceneManager.LoadScene(sceneIndex);
    }

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
        if(GameState == State.Running)
        {
            GameState = State.End;
        }
    }

    private IEnumerator LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount - 1)
        {
            GameState = State.Loading;
            yield return new WaitForSeconds(NextLevelDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1)
        {
            ShowEndMenu();
        }
    }

    internal void PauseGame()
    {
        if (GameState == State.Running || GameState == State.MainMenu)
        {
                GameState = State.Paused;
        }
    }

    internal void ResumeGame()
    {
        if (GameState == State.Paused)
        {
            if (SceneManager.GetActiveScene().name == "Test Scene")
            {
                GameState = State.MainMenu;
            }
            else
            {
                GameState = State.Running;
            }
        }
    }

    internal void StartNextLevel()
    {
        if (GameState == State.Running)
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    internal void ExitGame()
    {
        Application.Quit();
    }

    #endregion Methods

    #region Unity Methods

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameState = State.MainMenu;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        BGMusic = Resources.Load<AudioClip>("Audio/GRBackgroundMusic");
        TurnSound = Resources.Load<AudioClip>("Audio/RotateSound");
        WinSound = Resources.Load<AudioClip>("Audio/YouWinSound");
        MenuSuiteTemplate = Resources.Load<GameObject>("Prefabs/MenuSuite");
        WinMat = Resources.Load<Material>("Materials/RecieverOn");
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {

    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        EarCameraObject = GameObject.Find("Camera (ears)");
        if (CameraObject)
        {
            Camera = CameraObject.GetComponent<Camera>();
        }
        if (EarCameraObject)
        {
            Ears = EarCameraObject.GetComponent<AudioListener>();
        }
        if (EarCameraObject)
        {
            BGMusicSource = EarCameraObject.AddComponent<AudioSource>();
            BGMusicSource.playOnAwake = false;
            BGMusicSource.loop = true;
            BGMusicSource.clip = BGMusic;
            BGMusicSource.Play();
        }
        if (arg0.name == "Test Scene")
        {
            GameState = State.MainMenu;
        }
        else
        {
            GameState = State.Running;
        }
    }

    #endregion Unity Methods
    
}
