using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates {get {return startCoordinates;}}
    public Vector2Int DestinationCoordinates {get {return DestinationCoordinates;}}
    [SerializeField] Vector2Int destinationCoordinates;

    Node currentSearchNode;
    Node startNode;
    Node destinationNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int,Node> reached = new Dictionary<Vector2Int, Node>();


    Vector2Int[] directions = {Vector2Int.right,Vector2Int.left,Vector2Int.up,Vector2Int.down};
    GridManager gridManager;
    Dictionary<Vector2Int,Node> grid = new Dictionary<Vector2Int, Node>();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager !=null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];

        }   
    }

    void Start()
    {
        GetNewPath();
    }
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }
    void ExploreNeighborNodes()
    {
        List<Node> neighbors = new List<Node>();
  
        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;

            if(gridManager.Grid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(gridManager.Grid[neighborCoordinates]);
            }



        }
        foreach(Node neighbor in neighbors)
        {
            if(!reached.ContainsKey(neighbor.coordinates)&&neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }
    void BreadthFirstSearch(Vector2Int coordinates)
    {
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        frontier.Clear();
        reached.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates,grid[coordinates]);

        while(frontier.Count > 0 && isRunning == true)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighborNodes();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node CurrentNode = destinationNode;

        path.Add(CurrentNode);
        CurrentNode.isPath = true;

        while(CurrentNode.connectedTo != null)
        {
            CurrentNode = CurrentNode.connectedTo;
            path.Add(CurrentNode);
            CurrentNode.isPath = true;
        }

        path.Reverse();

        return path;
    }
    public bool willBlockPath(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            bool previoudState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = true;

            if(newPath.Count <=1)
            {
                GetNewPath();
                return true;
            }
        }
        return false;
    }
    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath",false,SendMessageOptions.DontRequireReceiver);
    }

}
