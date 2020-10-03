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

    public Command(int steps, int id, Direction movedir, Direction shootdir)
    {
        this.nbOfSteps = steps;
        this.unitID = id;
        this.directionOfMovement = movedir;
        this.directionOfShot = shootdir;
    }
}
