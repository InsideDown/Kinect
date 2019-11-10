using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform GameObjTrans;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameObjTrans.position.ToString());
        Debug.Log(GameObjTrans.localPosition.ToString("F4"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
