using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlaceable;

    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();

    public bool IsPlaceable()
    {
        return isPlaceable;
    }

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Start()
    {
     if(gridManager != null)
     {
        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        if(!isPlaceable)
        {
            gridManager.BlockNode(coordinates);
        }
     }   
    }

    void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.willBlockPath(coordinates))
        {
            bool isSucsessful = towerPrefab.CreateTower(towerPrefab, transform.position);
           
           if (isSucsessful)
           {
            gridManager.BlockNode(coordinates);
            pathfinder.NotifyReceivers();
           }
        }
    }
}
