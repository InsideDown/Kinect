using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTracker : MonoBehaviour
{
    public GameObject HandToFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = HandToFollow.transform.position;
    }
}
