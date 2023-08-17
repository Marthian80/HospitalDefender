using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{    
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    private List<Node> path = new List<Node>();
    private Gridmanager gridManager;
    private Pathfinding pathfinder;
    private Enemy enemy;
    private InfectionRate infectionRate;

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();
        pathfinder = FindObjectOfType<Pathfinding>();
        enemy = FindObjectOfType<Enemy>();
        infectionRate = FindObjectOfType<InfectionRate>();

        //Activate event listener
        EventManager.StartListening("RecalculatePath", RecalculatePath);
    }

    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }       

    private void ReturnToStart()
    {
        pathfinder.SetRandomStartPosition();
        pathfinder.SetRandomDestinationPosition();
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.CurrentStartCoordinates);
    }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = pathfinder.CurrentStartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPathRoutine());
    }

    private void FinishPath(Vector2 position)
    {
        gameObject.SetActive(false);
                
        if (CheckForPatientToInfect(position))
        {
            infectionRate.InfectPatientAtLocation(position);
            enemy.StealGold();
        }
    }

    private bool CheckForPatientToInfect(Vector2 position)
    {
        var finishedPositionCoordinates = gridManager.GetCoordinatesFromPosition(position);
        var node = gridManager.GetNode(finishedPositionCoordinates);

        return node.hasPatient;
    }

    private IEnumerator FollowPathRoutine()
    {
        Vector2 endPosition = transform.position;
        Vector2 startPosition = transform.position;

        for (int i = 1; i < path.Count; i++)
        {
            startPosition = transform.position;
            endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector2.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath(endPosition);
    }
}
