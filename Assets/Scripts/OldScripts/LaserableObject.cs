using UnityEngine;

namespace oldScripts
{
    public class LaserableObject : MonoBehaviour
    {
        #region Variables

        public bool IsEndGoal;
        public bool DestroyOnLasered;
        public bool Lasered;
        public LaserEmittingObject2 LaseringObject;
        protected GameController Controller;

        #endregion Variables

        #region Methods

        /// <summary>
        /// Called by a LaserEmittingObject when it hits a LaserableObject
        /// </summary>
        /// <param name="_LaseringObject"></param>
        public virtual void OnLasered(GameObject _LaseringObject)
        {
            LaseringObject = _LaseringObject.GetComponent<LaserEmittingObject2>();
            if (DestroyOnLasered)
            {
                Destroy(gameObject);
            }
            if (IsEndGoal)
            {
                Controller.EndLevel();
            }
        }

        #endregion Methods

        #region Unity Methods

        // Use this for initialization
        private void Start()
        {
            Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion Unity Methods
    }
}