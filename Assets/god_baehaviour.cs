using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class god_baehaviour : MonoBehaviour
{
    Queue<List<Command>> commandsQueue = new Queue<List<Command>>();
    GameObject[] units;
    Vector3[] unitPositions;
    bool turnHasEnded;
    public float speedOfUnits = 10;
    public GameObject dayText;
    
    private int day = 0;
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
        commandsQueue.Enqueue(new List<Command>());
        commandsQueue.Enqueue(new List<Command>());
        //initialise 

    }

    void die(GameObject unit) {
        Destroy(unit);
        units = GameObject.FindGameObjectsWithTag("Unit");
    }

    // Update is called once per frame
    void Update()
    {        
        if (turnHasEnded)
        {
            day += 1;
            //update day in text
            dayText.GetComponent<Text>().text = "Day " + day;
            //receive commands for turn
            List<Command> listOfCommands = new List<Command>();
            foreach(GameObject unit in units)
            {
                listOfCommands.Add(unit.GetComponent<unit_behaviour>().getDirections());
                Debug.Log("list of commands is " + unit.GetComponent<unit_behaviour>().getDirections());
            }
            //store lists of commands in a queue, one list entry per day/turn
            commandsQueue.Enqueue(listOfCommands);
            //AI commands
            List<Command> listOfAICommands = new List<Command>();
            foreach(GameObject unit in units)
            {
                if(unit.GetComponent<unit_behaviour>().allegiance == unit_behaviour.Allegiance.AI)
                {
                    listOfAICommands.Add(getRandomCommand(unit));
                }
            }
            commandsQueue.Enqueue(listOfAICommands);
            //execute the commands through the queue, one day at a time
            List<Command> commandsToExecute = commandsQueue.Dequeue();
            
            foreach (Command command in commandsToExecute)
            {
                Debug.Log("to execute " + command);
                ExecuteCommand(command);
            }
            //Execute the AI commands
            List<Command> aiCommandsToExecute = commandsQueue.Dequeue();
            foreach (Command command in aiCommandsToExecute)
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

    public Command getRandomCommand(GameObject unit)
    {
        unit_behaviour script = unit.GetComponent<unit_behaviour>();
        int nbSteps = Random.Range(0,script.lengthOfMovement-1);
        int directionOfMov;
        if (script.diagonal)
            directionOfMov = Random.Range(4, 8);
        else
            directionOfMov = Random.Range(0, 4);
        return new Command(nbSteps, script.id, (Command.Direction) directionOfMov, (Command.Direction) Random.Range(0, 8));       
    }

    public void setTurnEndedToTrue()
    {
        turnHasEnded = true;
    }

    void ExecuteCommand(Command command)
    {
        GameObject unitToCommand = null;
        foreach(GameObject unit in units)
        {
            if (unit.GetComponent<unit_behaviour>().id == command.unitID)
            {
                unitToCommand = unit;
            }
        }
        // Unit died, sorry
        if (unitToCommand == null) return;
        //turn unit before moving
        unitToCommand.GetComponent<unit_behaviour>().turn(command.directionOfMovement);
        //get actual number of steps possible
        Debug.Log("number of steps is before " + command.nbOfSteps);
        getCollisionLocationAndUpdateSteps(command.directionOfMovement, unitToCommand, command);
        Debug.Log("cell of collision is "+getCollisionLocationAndUpdateSteps(command.directionOfMovement, unitToCommand, command));
        Debug.Log("number of steps is now "+command.nbOfSteps);
        //movement
        StartCoroutine(moveUnit(unitToCommand, command));               
    }

    Vector2Int getCollisionLocationAndUpdateSteps(Command.Direction direction, GameObject unitToCommand, Command command)
    {
        Vector2Int cellPosition = getCellPosition(unitToCommand);  
        for(int i = 0; i < 22; i++)
        {
            Vector2Int nextCellLocation = new Vector2Int(cellPosition.x+directionToMutatorVector[direction].x,
                cellPosition.y+directionToMutatorVector[direction].y);
            if (!(Mathf.Abs(nextCellLocation.x) <= 5 && Mathf.Abs(nextCellLocation.y) <= 5))
            {
                if (i < command.nbOfSteps)
                    command.nbOfSteps = i;
                return nextCellLocation;
            }         
            foreach(GameObject unit in units)
            {
                if(unit.GetComponent<unit_behaviour>().id != unitToCommand.GetComponent<unit_behaviour>().id 
                    && getCellPosition(unit) == getCellPosition(unitToCommand))
                {
                    if (i < command.nbOfSteps)
                        command.nbOfSteps = i;
                    return nextCellLocation;
                }
            }
            cellPosition = nextCellLocation;
        }
        return new Vector2Int(0,0);
    }

    Vector2Int getCellPosition(GameObject unitToCommand)
    {
        return (Vector2Int) GameObject.Find("Map").GetComponent<Grid>().WorldToCell(unitToCommand.GetComponent<Transform>().position + new Vector3(2,1,0));
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
        Debug.Log(unitPosition != targetPosition);
        //unitToCommand.GetComponent<Transform>().position = Vector2.MoveTowards(unitPosition, new Vector2(0,0), 10);
        while (Mathf.Abs(unitToCommand.GetComponent<Transform>().position.x - targetPosition.x) > 0.01 ||
            Mathf.Abs(unitToCommand.GetComponent<Transform>().position.y - targetPosition.y) > 0.01)
        {
            Debug.Log("moving");
            //unitToCommand.GetComponent<Transform>().position = Vector2.MoveTowards(unitPosition, new Vector2(0,0), step);
            unitToCommand.GetComponent<Transform>().position = Vector3.MoveTowards(unitPosition, targetPosition, step);
            unitPosition = unitToCommand.GetComponent<Transform>().position;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
