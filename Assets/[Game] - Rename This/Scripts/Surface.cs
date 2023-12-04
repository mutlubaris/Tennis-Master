using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SurfaceType { PlayerCourt, OpponentCourt, Outside, TennisNet }

public class Surface : MonoBehaviour
{
    public SurfaceType SurfaceType;
}
