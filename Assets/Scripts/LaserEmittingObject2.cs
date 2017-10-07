using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserEmittingObject2 : LaserableObject {
    
    private GameObject LaserTemplate;
    private GameObject OutboundLaser;
    private Vector3 InactiveLaserScale;
    public float HeightTweak = 1.1316f;
    public float AngleTweak = 0;
    public bool IsLaserSource = false;
    public bool IsLaserActive = false;
    public RaycastHit[] HitObjects = null;
    public RaycastHit FirstHit;
    public LaserableObject LaseredObject = null;
    public ReflectorObject2 LaseredReflector;

    // Use this for initialization
    void Start () {
        CommonStart();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (IsLaserSource)
        {
            SendLaser();
        }
	}

    public override void OnLasered(GameObject _LaseringObject)
    {
        base.OnLasered(_LaseringObject);
    }

    public void SendLaser()
    {
        SendLaser(gameObject.transform.forward);
    }

    public void SendLaser(Vector3 Direction)
    {
        Vector3 RayOrigin = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + .1f, gameObject.transform.position.z);

        Ray Cast = new Ray(RayOrigin, Direction);
        Debug.DrawRay(RayOrigin, Direction);
        HitObjects = Physics.RaycastAll(Cast);
        OutboundLaser.transform.rotation = Quaternion.LookRotation(Direction);
        if(HitObjects != null)
        {
            List<RaycastHit> HitObjectsList = new List<RaycastHit>(HitObjects);
            List<RaycastHit> SortedHitObjectsList = HitObjectsList.OrderBy(R => R.distance).ToList();
            foreach(RaycastHit Hit in SortedHitObjectsList)
            {
                if(Hit.collider.gameObject != gameObject)
                {
                    FirstHit = Hit;
                    LaseredObject = Hit.collider.gameObject.GetComponent<LaserableObject>();
                    LaseredReflector = Hit.collider.gameObject.GetComponent<ReflectorObject2>();
                    break;
                }
            }
            ShowLaser(FirstHit.distance);

            if (LaseredReflector != null)
            {
                LaseredReflector.OnLasered(gameObject);
            }
            else if (LaseredObject != null)
            {
                LaseredObject.OnLasered(gameObject);
            }
        }
        else
        {
            LaseredObject = null;
            ShowLaser(100);
        }
    }

    public void HideLaser()
    {
        OutboundLaser.SetActive(false);
        OutboundLaser.transform.localScale = InactiveLaserScale;
    }

    public void ShowLaser(float Length)
    {
        OutboundLaser.SetActive(true);
        OutboundLaser.transform.localScale = new Vector3(1, 1, Length);
    }

    public void CommonStart()
    {
        if (HeightTweak < 0)
        {
            HeightTweak = gameObject.transform.localScale.y / 2;
        }
        InactiveLaserScale = new Vector3(0, 0, 0);
        Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        LaserTemplate = Controller.GetLaserTemplate();
        OutboundLaser = Instantiate(LaserTemplate, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + HeightTweak, gameObject.transform.position.z), gameObject.transform.rotation);
        OutboundLaser.transform.localScale = InactiveLaserScale;
        OutboundLaser.tag = "Laser";
        OutboundLaser.layer = 8;
    }
}
