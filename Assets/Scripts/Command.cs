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
    private int steps;
    private int id;
    private Direction movedir;
    private Direction shootdir;

    public Command(int steps, int id, Direction movedir, Direction shootdir)
    {
        this.steps = steps;
        this.id = id;
        this.movedir = movedir;
        this.shootdir = shootdir;
    }
}
