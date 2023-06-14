using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public Dictionary<Vector2Int, Node> Grid
    {
        get { return grid; }
    }

    private void Awake()
    {
        CreateGrid();
    }

    public void ResetNodes()
    {
        foreach (var node in grid)
        {
            node.Value.connectedTo = null;
            node.Value.isExplored = false;
            node.Value.isPath = false;
        }
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector2 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x);
        coordinates.y = Mathf.RoundToInt(position.y);

        return coordinates;
    }

    public Vector2 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector2 position = new Vector2();
        position.x = coordinates.x;
        position.y = coordinates.y;

        return position;
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
