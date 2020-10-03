﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    enum Direction{
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
    public Direction directionOfMovement;
    public Direction directionOfShot;
}
