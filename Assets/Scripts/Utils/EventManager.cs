using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{

    protected EventManager() { }

    public delegate void KinectAction();
    public static event KinectAction OnUserFoundEvent;
    public static event KinectAction OnAllUsersLostEvent;

    public delegate void LightColorAction(string colorStr);
    public static event LightColorAction OnLightColorEvent;

    public delegate void LightAction();
    public static event LightAction OnLightTriggerEvent;

    public void LightTriggerEvent()
    {
        if (OnLightTriggerEvent != null)
            OnLightTriggerEvent();
    }

    public void UserFoundEvent()
    {
        if (OnUserFoundEvent != null)
            OnUserFoundEvent();
    }

    public void AllUsersLostEvent()
    {
        if (OnAllUsersLostEvent != null)
            OnAllUsersLostEvent();
    }

    public void LightColorEvent(string colorStr)
    {
        if (OnLightColorEvent != null)
            OnLightColorEvent(colorStr);
    }
}