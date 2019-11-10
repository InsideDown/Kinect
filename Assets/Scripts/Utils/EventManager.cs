using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{

    protected EventManager() { }

    public delegate void KinectAction();
    public static event KinectAction OnUserFoundEvent;
    public static event KinectAction OnAllUsersLostEvent;

    public delegate void VideoStartAction(string curVideoItem);
    public static event VideoStartAction OnVideoStartEvent;
    public static event VideoStartAction OnRingPlacedEvent;

    public delegate void KeyboardAction();
    public static event KeyboardAction OnRedTeamWin;
    public static event KeyboardAction OnGreenTeamWin;

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

    public void RedTeamWin()
    {
        if (OnRedTeamWin != null)
            OnRedTeamWin();
    }

    public void GreenTeamWin()
    {
        if (OnGreenTeamWin != null)
            OnGreenTeamWin();
    }

    public void VideoStartEvent(string curVideoItem)
    {
        if (OnVideoStartEvent != null)
            OnVideoStartEvent(curVideoItem);
    }

    public void RingPlacedEvent(string curVideoItem)
    {
        if (OnRingPlacedEvent != null)
            OnRingPlacedEvent(curVideoItem);
    }
}