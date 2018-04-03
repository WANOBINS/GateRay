using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A class to be included in the UI Suite so the functions are always available
/// </summary>
public class UIFunctions : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void RequestNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
        {
            GameObject.Find("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        RequestNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}