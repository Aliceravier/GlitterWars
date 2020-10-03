using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    enum Direction
    {
        North,
        South,
        East,
        West,
        SouthWest,
        SouthEast,
        NorthWest,
        NorthEast
    }

    private int nbOfSteps;
    private Direction directionOfMovement;
    private Direction directionOfShot;
}

public class god_baehaviour : MonoBehaviour
{
    Queue<List<Command>> commandsQueue = new Queue<List<Command>>();
    GameObject[] units;

    // Start is called before the first frame update
    void Start()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        commandsQueue.Enqueue(new List<Command>(null));
        commandsQueue.Enqueue(new List<Command>(null));
    }

    // Update is called once per frame
    void Update()
    {
        bool turnHasEnded = true;
        if (turnHasEnded) //change to something which tells if a turn is over
        {
            //receive commands for turn
            List<Command> listOfCommands = new List<Command>();
            foreach(GameObject unit in units)
            {
                listOfCommands.Add(unit.GetComponent<unit_behaviour>().command);
            }
            //store lists of commands in a queue, one list entry per day/turn
            commandsQueue.Enqueue(listOfCommands);

            //execute the commands through the queue, one day at a time
            List<Command> commandsToExecute = commandsQueue.Dequeue();
            foreach(Command command in commandsToExecute)
            {
                ExecuteCommand(command);
            }
        }
    }

    void ExecuteCommand(Command command)
    {
        command.
    }
}
