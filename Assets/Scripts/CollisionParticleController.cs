using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticleController : MonoBehaviour
{
    public ParticleSystem CollisionParticles;
    public List<ParticleSystem> CollisionParticleColors = new List<ParticleSystem>();

    private void Awake()
    {
        if (CollisionParticles == null)
            throw new System.Exception("CollisionParticles must be defined in CollisionParticleController");
    }

    private void OnEnable()
    {
        EventManager.OnLightColorEvent += EventManager_OnLightTriggerEvent;
        
    }

    private void OnDisable()
    {
        EventManager.OnLightColorEvent -= EventManager_OnLightTriggerEvent;
    }

    private void EventManager_OnLightTriggerEvent(string colorStr)
    {
        Color particleColor = Color.red;
        switch (colorStr)
        {
            case "green":
                particleColor = Color.green;
                break;
            case "blue":
                particleColor = Color.blue;
                break;
        }

        if(CollisionParticleColors.Count > 0)
        {
            for(int i=0;i<CollisionParticleColors.Count;i++)
            {
                ParticleSystem curSystem = CollisionParticleColors[i];
                curSystem.startColor = particleColor;
            }
        }

        CollisionParticles.Play();
    }
}
