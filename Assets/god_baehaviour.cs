using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class god_baehaviour : MonoBehaviour
{
    Queue<List<Command>> commandsQueue = new Queue<List<Command>>();
    GameObject[] units;
    Vector3Int[] unitPositions;
    Dictionary<Command.Direction, Vector2Int> directionToMutatorVector;
    bool turnHasEnded;

    // Start is called before the first frame update
    void Start()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        commandsQueue.Enqueue(new List<Command>());
        commandsQueue.Enqueue(new List<Command>());
        //initialise unit positions
    }

    // Update is called once per frame
    void Update()
    {        
        if (turnHasEnded)
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
        turnHasEnded = false;
    }

    public void setTurnEndedToTrue()
    {
        turnHasEnded = true;
    }

    void ExecuteCommand(Command command)
    {
        GameObject unitToCommand;
        Vector2 unitPosition = new Vector2();
        foreach(GameObject unit in units)
        {
            if (unit.GetComponent<unit_behaviour>().id == command.unitID) 
                unitToCommand = unit;
        }
        //movement
        foreach(Vector3Int position in unitPositions)
        {
            if (position.z == command.unitID)
                unitPosition = new Vector2(position.x, position.y);
        }
        Vector3 sizeOfOneTile = GameObject.Find("Map").GetComponent<Grid>().cellSize;
        float widthOfOneTile = sizeOfOneTile.x;
        Vector2 targetPosition = new Vector2(command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].x,
            command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].y);
        Vector2.MoveTowards(unitPosition, targetPosition, command.nbOfSteps * widthOfOneTile);



    }
}
