using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseTracker : MonoBehaviour
{

    public Transform RightHandPos;
    public Transform RightElbowPos;
    public Transform RightShoulderPos;
    public Transform RightShoulderRotation;
    public Transform LeftHandPos;
    public Transform LeftElbowPos;
    public Transform LeftShoulderPos;
    public Transform LeftShoulderRotation;

    public GameObject SnowballHolder;
    public GameObject SnowballPrefab;

    //the IcyBlastAngle ensures that an arm is in a straight enough angle from the shoulder to be considered in firing position
    public float IcyBlastAngle = 10.0f;
    public float ResetBlastDistance = 0.1f; //how close does our arm need to be to our shoulder in order to consider it reset? 
    public float IcyBlastSpeed = 5.0f; //larger the value, slower it will be

    public ParticleSystem LeftParticle;
    public ParticleSystem RightParticle;

    //the IcyBlastTimeout forces the user to "reset" their firing power by putting their arm down
    private bool AllowRightBlast = true;
    private bool AllowLeftBlast = true;


    private void Awake()
    {
        LeftParticle.Pause();
        RightParticle.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (RightHandPos == null)
            throw new System.Exception("A RightHandPos must be defined in PoseTracker");
        if (RightElbowPos == null)
            throw new System.Exception("A RightElbowPos must be defined in PoseTracker");
        if (RightShoulderPos == null)
            throw new System.Exception("A RightShoulderPos must be defined in PoseTracker");
        if (LeftHandPos == null)
            throw new System.Exception("A LeftHandPos must be defined in PoseTracker");
        if (LeftElbowPos == null)
            throw new System.Exception("A LeftElbowPos must be defined in PoseTracker");
        if (LeftShoulderPos == null)
            throw new System.Exception("A LeftShoulderPos must be defined in PoseTracker");

        LeftParticle.Play();
        RightParticle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //only do anything if we're initialized and we see users
        KinectManager kinectManager = KinectManager.Instance;
        if (kinectManager && kinectManager.IsInitialized())
        {
            if (kinectManager.IsUserDetected())
            {
                CheckRightGestures();
                CheckLeftGestures();
            }
        }
    }

    void CheckRightGestures()
    {
        //position our rightShoulderTracker
        RightShoulderRotation.position = RightShoulderPos.position;
        RightShoulderRotation.LookAt(RightHandPos);

        Vector3 targetDir = RightHandPos.position - RightShoulderPos.position;
        float shoulderHandAngle = Vector3.Angle(targetDir, RightShoulderPos.forward);

        Vector3 targetDir2 = RightElbowPos.position - RightShoulderPos.position;
        float shoulderElbowAngle = Vector3.Angle(targetDir2, RightShoulderPos.forward);

        float angle = Mathf.Abs(shoulderElbowAngle - shoulderHandAngle);

        if ((angle <= IcyBlastAngle) && AllowRightBlast)
        {
            //we are in a straight angle - now we need to make sure we're not just pointing down
            float shoulder = RightShoulderPos.position.x;
            float hand = RightHandPos.position.x;
            if (Mathf.Abs(hand - shoulder) > ResetBlastDistance * 2)
            {
                FireIceBlast("right");
            }
        }

        if (!AllowRightBlast)
            CheckBlastReset("right");

    }

    void CheckLeftGestures()
    {
        //position our rightShoulderTracker
        LeftShoulderRotation.position = LeftShoulderPos.position;
        LeftShoulderRotation.LookAt(LeftHandPos);

        Vector3 targetDir = LeftHandPos.position - LeftShoulderPos.position;
        float shoulderHandAngle = Vector3.Angle(targetDir, LeftShoulderPos.forward);

        Vector3 targetDir2 = LeftElbowPos.position - LeftShoulderPos.position;
        float shoulderElbowAngle = Vector3.Angle(targetDir2, LeftShoulderPos.forward);

        float angle = Mathf.Abs(shoulderElbowAngle - shoulderHandAngle);

        if ((angle <= IcyBlastAngle) && AllowLeftBlast)
        {
            //we are in a straight angle - now we need to make sure we're not just pointing down
            float shoulder = LeftShoulderPos.position.x;
            float hand = LeftHandPos.position.x;
            if (Mathf.Abs(hand - shoulder) > ResetBlastDistance * 2)
            {
                FireIceBlast("left");
            }
        }

        if (!AllowLeftBlast)
            CheckBlastReset("left");

    }

    void FireIceBlast(string direction)
    {
        Transform startPos = RightHandPos;
        Transform rotationHolder = RightShoulderRotation;
        
        if(direction == "right")
        {
            AllowRightBlast = false;
            
        }else if(direction == "left")
        {
            startPos = LeftHandPos;
            rotationHolder = LeftShoulderRotation;
            AllowLeftBlast = false;
        }

        GameObject snowballObj = Instantiate(SnowballPrefab, SnowballHolder.transform, false);
        snowballObj.transform.position = startPos.position;
        //snowballObj.transform.rotation = RightShoulderRotation.rotation;

        Vector3 addDistanceToDirection = rotationHolder.rotation * snowballObj.transform.forward * 10.0f;
        Vector3 destination = snowballObj.transform.position + addDistanceToDirection;
        snowballObj.transform.DOMove(destination, IcyBlastSpeed).OnComplete(()=>
        {
            RemoveObj(snowballObj);
        });
    }

    void RemoveObj(GameObject obj)
    {
        Destroy(obj);
    }

    void CheckBlastReset(string direction)
    {
        float shoulder = RightShoulderPos.position.x;
        float hand = RightHandPos.position.x;

        if(direction == "left")
        {
            shoulder = LeftShoulderPos.position.x;
            hand = LeftHandPos.position.x;
        }

        if(Mathf.Abs(hand-shoulder) < ResetBlastDistance)
        {
            if (direction == "right")
            {
                AllowRightBlast = true;
            }else if(direction == "left")
            {
                AllowLeftBlast = true;
            }
        }
    }
    
}
