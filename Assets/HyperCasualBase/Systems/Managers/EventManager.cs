using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public static class EventManager 
{
    public static UnityEvent OnCorrectAnswer = new UnityEvent();
    public static UnityEvent OnAskQuestion = new UnityEvent();
    public static TennisBallHitEvent OnTennisBallHit = new TennisBallHitEvent();
    public static PointFinishedEvent OnPointFinished = new PointFinishedEvent();
    public static UnityEvent OnPointReset = new UnityEvent();
    public static UnityEvent OnPointStarted = new UnityEvent();
}

public class PointFinishedEvent : UnityEvent<CharacterType> { }
