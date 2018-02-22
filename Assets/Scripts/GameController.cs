using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    public double MirrorTurnIncrement = 22.5;
    public double NextLevelDelay = 5;

    private AudioClip _BGMusic;
    private AudioClip _TurnSound;
    private AudioClip _WinSound;

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
    }

    public AudioClip TurnSound
    {
        get
        {
            return _TurnSound;
        }
    }

    public AudioClip WinSound
    {
        get
        {
            return _WinSound;
        }
    }

    #endregion Variables

    #region Methods

    public void FinishLevel()
    {
    }

    #endregion Methods

    #region Unity Methods

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        _BGMusic = Resources.Load<AudioClip>("Audio/GRBackgroundMusic");
        _TurnSound = Resources.Load<AudioClip>("Audio/RotateSound");
        _WinSound = Resources.Load<AudioClip>("Audio/YouWinSound");

        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        EarCameraObject = GameObject.Find("Camera (ears)");

        Camera = CameraObject.GetComponent<Camera>();
        Ears = EarCameraObject.GetComponent<AudioListener>();
    }

    // Use this for initialization
    private void Start()
    {
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
}