using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private List<Vector2Int> startCoordinates = new List<Vector2Int>();    
    [SerializeField] private List<Vector2Int> tankCoordinates = new List<Vector2Int>();

    private Vector2Int currentStartCoordinates;
    public Vector2Int CurrentStartCoordinates { get { return currentStartCoordinates; } }   
    private Vector2Int currentDestinationCoordinates;    

    private Node startNode;
    private Node destinationNode;
    private Node currentSearchNode;

    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();
    private Queue<Node> frontier = new Queue<Node>();

    private Gridmanager gridManager;
    private TargetControl targetControl;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    private readonly Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();
        targetControl = FindObjectOfType<TargetControl>();

        if (gridManager!= null )
        {
            grid = gridManager.Grid;
            SetRandomStartPosition();
            SetRandomDestinationPosition();
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public void SetRandomStartPosition()
    {
        currentStartCoordinates = startCoordinates[Random.Range(0, startCoordinates.Count)];
        startNode = grid[currentStartCoordinates];
    }

    public Vector2Int SetRandomDestinationPosition()
    {
        var activeTargets = targetControl.GetActivePatientTargets();
        currentDestinationCoordinates = activeTargets[Random.Range(0, activeTargets.Count)].coordinates;
        destinationNode = grid[currentDestinationCoordinates];

        return currentDestinationCoordinates;
    }

    public void SetRandomTankDestinationPosition()
    {
        currentDestinationCoordinates = tankCoordinates[Random.Range(0, tankCoordinates.Count)];
        destinationNode = grid[currentDestinationCoordinates];
    }

    public void SetStartPositionOnTheseCoordinates(Vector2Int startCoordinates)
    {
        currentStartCoordinates = startCoordinates;
        startNode = grid[currentStartCoordinates];
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(currentStartCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int currentCoordinates)
    {
        gridManager.ResetNodes();
        BreadFirstSearch(currentCoordinates);
        return BuildPath();
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }

    private void BreadFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExpoloreNeighbours();
            if (currentSearchNode.coordinates == currentDestinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }
        path.Reverse();

        return path;
    }

    private void ExpoloreNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (var direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighborCoords))
            {
                neighbours.Add(grid[neighborCoords]);
            }
        }

        foreach (var neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo = currentSearchNode;
                reached.Add(neighbour.coordinates, neighbour);
                frontier.Enqueue(neighbour);
            }
        }
    }

    public void NotifyReceivers()
    {
        EventManager.TriggerEvent("RecalculatePath", false);
    }
}
