using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class god_baehaviour : MonoBehaviour
{
    Queue<List<Command>> commandsQueue = new Queue<List<Command>>();
    GameObject[] units;
    Vector3[] unitPositions;
    bool turnHasEnded;
    public float speedOfUnits = 10;
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
                Debug.Log("list of commands is " + unit.GetComponent<unit_behaviour>().getDirections());
            }
            //store lists of commands in a queue, one list entry per day/turn
            commandsQueue.Enqueue(listOfCommands);
            

            //execute the commands through the queue, one day at a time
            List<Command> commandsToExecute = commandsQueue.Dequeue();
            
            foreach (Command command in commandsToExecute)
            {
                Debug.Log("to execute " + command);
                ExecuteCommand(command);
            }
        foreach (GameObject unit in units)
        {
            unit.GetComponent<unit_behaviour>().resetShootAndMove();
        }
        turnHasEnded = false;
        }
        
    }

    public void setTurnEndedToTrue()
    {
        turnHasEnded = true;
    }

    void ExecuteCommand(Command command)
    {
        Debug.Log("in execute command");
        GameObject unitToCommand = null;
        foreach(GameObject unit in units)
        {
            if (unit.GetComponent<unit_behaviour>().id == command.unitID)
            {
                unitToCommand = unit;
                Debug.Log("unit to command is " + unitToCommand);
            }
        }
        //get actual number of steps possible
        updateNbOfPossibleSteps(command, unitToCommand);
        
        //movement
        StartCoroutine(moveUnit(unitToCommand, command));
                
    }

    void updateNbOfPossibleSteps(Command command, GameObject unitToCommand)
    {
        Vector3Int cellPosition = getCellPosition(unitToCommand);  
        Debug.Log(cellPosition);
    }

    Vector3Int getCellPosition(GameObject unitToCommand)
    {
        return GameObject.Find("Map").GetComponent<Grid>().WorldToCell(unitToCommand.GetComponent<Transform>().position + new Vector3(2,1,0));
    }


    IEnumerator moveUnit(GameObject unitToCommand, Command command)
    {
        Vector3 unitPosition = unitToCommand.GetComponent<Transform>().position;
        Vector3 sizeOfOneTile = GameObject.Find("Map").GetComponent<Grid>().cellSize;
        float widthOfOneTile = sizeOfOneTile.x;
        Vector3 targetPosition = new Vector3(unitPosition.x + command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].x,
            unitPosition.y + command.nbOfSteps * widthOfOneTile * directionToMutatorVector[command.directionOfMovement].y,
            unitPosition.z);
        float step = speedOfUnits * Time.deltaTime;
        Debug.Log(unitToCommand.GetComponent<Transform>().position.x != targetPosition.x
            && unitToCommand.GetComponent<Transform>().position.y != targetPosition.y);
        //unitToCommand.GetComponent<Transform>().position = Vector2.MoveTowards(unitPosition, new Vector2(0,0), 10);
        int i = 0;
        while (i < 100)
        {
            Debug.Log("moving");
            //unitToCommand.GetComponent<Transform>().position = Vector2.MoveTowards(unitPosition, new Vector2(0,0), step);
            unitToCommand.GetComponent<Transform>().position = Vector3.MoveTowards(unitPosition, targetPosition, step);
            unitPosition = unitToCommand.GetComponent<Transform>().position;
            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }
}
