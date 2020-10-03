using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public enum Direction{
        North,
        South,
        East,
        West,
        SouthWest,
        SouthEast,
        NorthWest,
        NorthEast
    }

    public int nbOfSteps;
    public int unitID;
    public Direction directionOfMovement;
    public Direction directionOfShot;
}
