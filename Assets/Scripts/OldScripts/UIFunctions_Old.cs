using UnityEngine;
using UnityEngine.SceneManagement;

namespace oldScripts
{
#pragma warning disable
    public class UIFunctions_Old : MonoBehaviour
    {
        #region Variables

        private GameController_Old Controller;

        #endregion Variables

        #region Methods

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

        #endregion Methods

        #region Unity Methods

        // Use this for initialization
        private void Start()
        {
            Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController_Old>();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion Unity Methods
    }
#pragma warning restore
}