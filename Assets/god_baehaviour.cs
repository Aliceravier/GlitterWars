using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class god_baehaviour : MonoBehaviour
{
    Queue<List<Command>> commandsQueue = new Queue<List<Command>>();
    GameObject[] units;
    Vector3[] unitPositions;
    bool turnHasEnded;
    Dictionary<Command.Direction, Vector2Int> directionToMutatorVector = new Dictionary<Command.Direction, Vector2Int>(){
        {Command.Direction.North, new Vector2Int(0,1)},
        {Command.Direction.NorthEast, new Vector2Int(1,1)},
        {Command.Direction.East, new Vector2Int(1,0)},
        {Command.Direction.SouthEast, new Vector2Int(1,-1)},
        {Command.Direction.South, new Vector2Int(0,-1)},
        {Command.Direction.SouthWest, new Vector2Int(-1,-1)},
        {Command.Direction.West, new Vector2Int(-1,0)},
        {Command.Direction.NorthWest, new Vector2Int(-1,1)}
    };



    // Start is called before the first frame update
    void Start()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        commandsQueue.Enqueue(new List<Command>());
        commandsQueue.Enqueue(new List<Command>());
        //initialise unit positions
        unitPositions = new Vector3[units.Length];
        int i = 0;
        foreach(GameObject unit in units)
        {
           unitPositions[i] = unit.GetComponent<Transform>().position;
           unitPositions[i].z = unit.GetComponent<unit_behaviour>().id;
           i++;
        }
        
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
                listOfCommands.Add(unit.GetComponent<unit_behaviour>().getDirections());
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
        foreach(Vector3 position in unitPositions)
        {
            if (position.z == command.unitID)
                unitPosition = new Vector2(position.x, position.y);
        }
        Vector3 sizeOfOneTile = GameObject.Find("Map").GetComponent<Grid>().cellSize;
        float widthOfOneTile = sizeOfOneTile.x;
        Debug.Log(sizeOfOneTile);
        Debug.Log(command.nbOfSteps);
        Debug.Log(directionToMutatorVector[command.directionOfMovement].x);
        Vector2 targetPosition = new Vector2(command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].x,
            command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].y);
        Vector2.MoveTowards(unitPosition, targetPosition, command.nbOfSteps * widthOfOneTile);



    }
}
