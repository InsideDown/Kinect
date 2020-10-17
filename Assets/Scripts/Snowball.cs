using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "LightController")
        {
            Debug.Log("TOGGLE THE LIGHTS");
            EventManager.Instance.LightTriggerEvent();
            Destroy(this.gameObject);
        }
    }
}
