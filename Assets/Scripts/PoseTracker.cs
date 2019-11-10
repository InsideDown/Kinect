using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseTracker : MonoBehaviour
{

    public Transform RightHandPos;
    public Transform RightElbowPos;
    public Transform RightShoulderPos;

    public GameObject SnowballHolder;
    public GameObject SnowballPrefab;

    //the IcyBlastAngle ensures that an arm is in a straight enough angle from the shoulder to be considered in firing position
    public float IcyBlastAngle = 10.0f;
    public float ResetBlastDistance = 0.1f; //how close does our arm need to be to our shoulder in order to consider it reset? 


    //the IcyBlastTimeout forces the user to "reset" their firing power by putting their arm down
    private bool AllowRightBlast = true;

    // Start is called before the first frame update
    void Start()
    {
        if (RightHandPos == null)
            throw new System.Exception("A RightHandPos must be defined in PoseTracker");
        if (RightElbowPos == null)
            throw new System.Exception("A RightElbowPos must be defined in PoseTracker");
        if (RightShoulderPos == null)
            throw new System.Exception("A RightShoulderPos must be defined in PoseTracker");
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
            }
        }
        /*
            Vector3 targetDir = RightHandPos.position - RightElbowPos.position;
        float angle = Vector3.Angle(targetDir, RightElbowPos.forward);
        Debug.Log(angle);
        */
    }

    void CheckRightGestures()
    {
        Vector3 targetDir = RightHandPos.position - RightShoulderPos.position;
        float shoulderHandAngle = Vector3.Angle(targetDir, RightShoulderPos.forward);

        Vector3 targetDir2 = RightElbowPos.position - RightShoulderPos.position;
        float shoulderElbowAngle = Vector3.Angle(targetDir2, RightShoulderPos.forward);

        RightShoulderPos.LookAt(RightHandPos);

        float newVal = Mathf.Abs(shoulderElbowAngle - shoulderHandAngle);

        if ((newVal <= IcyBlastAngle) && AllowRightBlast)
        {
            //we are in a straight angle - now we need to make sure we're not just pointing down
            float shoulder = RightShoulderPos.position.x;
            float hand = RightHandPos.position.x;
            if (Mathf.Abs(hand - shoulder) > ResetBlastDistance)
            {
                FireIceBlast("right");
            }
        }

        if (!AllowRightBlast)
            CheckBlastReset("right");

    }

    void FireIceBlast(string direction)
    {
        Transform startPos = RightHandPos;
        
        if(direction == "right")
        {
            Debug.Log("FIRE BLAST =======>");
            AllowRightBlast = false;
            
        }else if(direction == "left")
        {
            startPos = RightHandPos;
        }

        GameObject snowballObj = Instantiate(SnowballPrefab, SnowballHolder.transform, false);
        snowballObj.transform.position = startPos.position;


        /*
        Vector3 newRotation = Vector3.RotateTowards(RightHandPos.forward, RightShoulderPos.transform.position, 100, 180);
        RightHandPos.rotation = Quaternion.LookRotation(newRotation);
        */
    }

    void CheckBlastReset(string direction)
    {
        float shoulder = RightShoulderPos.position.x;
        float hand = RightHandPos.position.x;
        if(Mathf.Abs(hand-shoulder) < ResetBlastDistance)
        {
            if (direction == "right")
            {
                Debug.Log("resetting blast");
                AllowRightBlast = true;
            }
        }
    }
    
}
