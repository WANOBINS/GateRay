using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour {
    private GameController Controller;

    // Use this for initialization
    void Start () {
        Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called when Resume Game is clicked in the pause menu
    /// </summary>
    public void ResumeGame()
    {
        Controller.ResumeGame();
    }

    /// <summary>
    /// Loads the Main Menu
    /// </summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Quits the game, here to make Application.Quit callable from Canvas buttons
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
