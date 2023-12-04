using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TennisData
{
    public int GamesToWin = 1;
    
    public float PlayerSpeed = 7f;
    public float OpponentSpeed = 7f;
    
    public float OpponentHitX = 6f;
    public float OpponentHitY = 6f;
    public float OpponentHitZ = 23f;

    public float PlayerHitX = 6f;
    public float PlayerHitY = 6f;
    public float PlayerHitZ = 15f;

    public bool IsSmartAI = false;
}
