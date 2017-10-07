using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameController : MonoBehaviour {

    private GameObject LaserTemplate;
    private GameObject MenuSuiteTemplate;
    private GameObject MenuSuite;
    private GameObject PauseMenu;
    private GameObject EventSystem;
    private AudioClip ObjectRotate;
    private AudioClip LevelWin;
    private AudioClip Music;
    private AudioSource MusicSource;
    private AudioSource LaserableEmittingObjectAudioSource;
    private Material WinMat;
    private bool FirstUpdate = true;
    public bool IsMainMenu = false;
    public float NextLevelDelay = 5f;
    public float MirrorTurnIncrement = 22.5f;
    private bool LevelClosing = false;
    private bool Paused = false;
    private RaycastHit MouseClickHit;
    private Ray MouseClickRay;
    private GameObject CompletionMenu;

    void Awake () {
        ObjectRotate = Resources.Load<AudioClip>("Audio/RotateSound");
        LevelWin = Resources.Load<AudioClip>("Audio/YouWinSound");
        WinMat = Resources.Load<Material>("Materials/RecieverOn");
        Music = Resources.Load<AudioClip>("Audio/GRBackgroundMusic");
        LaserTemplate = Resources.Load<GameObject>("Prefabs/Laser");
        MenuSuiteTemplate = Resources.Load<GameObject>("Prefabs/MenuSuite");

    }

    private void Start()
    {
        LaserableEmittingObjectAudioSource = Camera.main.gameObject.AddComponent<AudioSource>();
        LaserableEmittingObjectAudioSource.clip = ObjectRotate;
        MusicSource = Camera.main.gameObject.AddComponent<AudioSource>();
        MusicSource.clip = Music;
        MusicSource.loop = true;
        MusicSource.volume = .1f;
        MusicSource.Play();
        MenuSuite = Instantiate(MenuSuiteTemplate);
        PauseMenu = MenuSuite.transform.FindChild("PauseMenu").gameObject;
        CompletionMenu = MenuSuite.transform.FindChild("CompletionScreen").gameObject;
        EventSystem = new GameObject();
        EventSystem.AddComponent<EventSystem>();
        EventSystem.AddComponent<StandaloneInputModule>();
        MouseClickRay = new Ray();
        PauseMenu.SetActive(false);
        CompletionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
        }
        if ((!IsMainMenu && !Paused) && !LevelClosing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(MouseClickRay,out MouseClickHit))
                {
                    if(MouseClickHit.collider.gameObject.tag == "TurnCollider")
                    {
                        GameObject MirrorHit = MouseClickHit.collider.gameObject.transform.parent.gameObject;
                        Transform MirrorTransform = MirrorHit.transform;
                        LaserableEmittingObjectAudioSource.Play();
                        MirrorTransform.localEulerAngles = new Vector3(MirrorHit.transform.rotation.eulerAngles.x, MirrorHit.transform.rotation.eulerAngles.y - MirrorTurnIncrement, MirrorHit.transform.rotation.eulerAngles.z);
                       // MirrorTransform.Rotate(MirrorHit.transform.rotation.eulerAngles.x, MirrorHit.transform.rotation.eulerAngles.y + 45, MirrorHit.transform.rotation.eulerAngles.z);
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                MouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(MouseClickRay, out MouseClickHit))
                {
                    if (MouseClickHit.collider.gameObject.tag == "TurnCollider")
                    {
                        GameObject MirrorHit = MouseClickHit.collider.gameObject.transform.parent.gameObject;
                        Transform MirrorTransform = MirrorHit.transform;
                        LaserableEmittingObjectAudioSource.Play();
                        MirrorTransform.localEulerAngles = new Vector3(MirrorHit.transform.rotation.eulerAngles.x, MirrorHit.transform.rotation.eulerAngles.y + MirrorTurnIncrement, MirrorHit.transform.rotation.eulerAngles.z);
                        // MirrorTransform.Rotate(MirrorHit.transform.rotation.eulerAngles.x, MirrorHit.transform.rotation.eulerAngles.y + 45, MirrorHit.transform.rotation.eulerAngles.z);
                    }
                }
            }
        }
        if (Paused)
        {
            if (PauseMenu.activeInHierarchy)
            {

            }
            else
            {
                PauseGame();
            }
        }
        else
        {
            if (PauseMenu.activeInHierarchy)
            {
                ResumeGame();
            }
            else
            {
                
            }
        }
	}


    /// <summary>
    /// Allows LaserEmittingObjects to get the laser prefab
    /// </summary>
    /// <returns></returns>
    public GameObject GetLaserTemplate()
    {
        return LaserTemplate;
    }

    /// <summary>
    /// Called by the final laser target in the level and the Play button of the Main Menu
    /// <para>TODO: This is what should trigger new Level Generation</para>
    /// </summary>
    public void EndLevel()
    {
        //throw new NotImplementedException();
        if (!LevelClosing)
        {
            if (!IsMainMenu)
            {
                LaserableEmittingObjectAudioSource.clip = LevelWin;
                LaserableEmittingObjectAudioSource.Play();
                GameObject.FindGameObjectWithTag("RecieverOrb").GetComponent<Renderer>().material = WinMat;
            }
            Time.timeScale = 0;
            StartCoroutine(LevelGeneration());
        }
    }

    /// <summary>
    /// Called when Escape is pressed
    /// </summary>
    public void PauseGame()
    {
        if (IsMainMenu)
        {
            Debug.LogWarning("Why would you try to Pause the Main Menu?");
            Paused = false;
        }
        else
        {
            Debug.Log("Pausing");
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Called when Resume Game is clicked in the pause menu
    /// </summary>
    public void ResumeGame()
    {
        Debug.Log("Resuming");
        Paused = false;
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public IEnumerator LevelGeneration()
    {
        LevelClosing = true;
        int NextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (IsMainMenu)
        {
            SceneManager.LoadScene(1);
            yield return new WaitForSeconds(0);
        }
        else if (NextScene >= SceneManager.sceneCountInBuildSettings)
        {
            CompletionMenu.SetActive(true);
        }
        else
        {
            yield return new WaitForSecondsRealtime(NextLevelDelay);
            SceneManager.LoadScene(NextScene);
        }
    }
}
