public class EmitterObject : LaserEmittingObject, ITurnable
{
    #region Variables

    private bool laserActive = false;

    #endregion Variables

    #region Methods

    public void TurnLeft()
    {
        transform.Rotate(transform.up, -12.5f);
    }

    public void TurnRight()
    {
        transform.Rotate(transform.up, 12.5f);
    }

    #endregion Methods

    #region Unity Methods

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion Unity Methods
}