using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMover : MonoBehaviour
{    
    [SerializeField][Range(0f, 5f)] float speed = 1f;

    private List<Node> path = new List<Node>();
    private Gridmanager gridManager;
    private Pathfinding pathfinder;
    private Enemy enemy;
    private BacteriaSpawner bacteriaSpawner;
    private Vector2Int currentTarget;

    private bool slownessTriggered = false;    

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();
        pathfinder = FindObjectOfType<Pathfinding>();
        enemy = FindObjectOfType<Enemy>();
        bacteriaSpawner = FindObjectOfType<BacteriaSpawner>();        

        //Activate event listener
        EventManager.StartListening("RecalculatePath", RecalculatePath);
        Timer.Instance.onTimerStopped += TimerStopped;
        Timer.Instance.onTimerResumed += TimerResumed;
    }

    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
        slownessTriggered = false;
        bacteriaSpawner.enemyInfectedTarget += OtherEnemyInfectedTarget;
    }

    //If other enemy already infected this enemy's target, set new target
    private void OtherEnemyInfectedTarget(Vector2 otherEnemyTarget)
    {
        if (currentTarget.x == (int)otherEnemyTarget.x && currentTarget.y == (int)otherEnemyTarget.y)
        {
            StartNewPath(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        }
    }

    private void OnDestroy()
    {
        Timer.Instance.onTimerStopped -= TimerStopped;
        Timer.Instance.onTimerResumed -= TimerResumed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();
        if (tile && tile.SlowEnemies)
        {
            slownessTriggered = true;
            speed = speed / 2;
        }
        else if (tile && !tile.SlowEnemies && slownessTriggered)
        {
            slownessTriggered = false;
            speed = speed * 2;
        }
    }

    private void StartNewPath(Vector2Int startCoordinates)
    {
        if (gameObject.activeInHierarchy)
        {
            pathfinder.SetStartPositionOnTheseCoordinates(startCoordinates);
            currentTarget = pathfinder.SetRandomDestinationPosition();
            transform.position = gridManager.GetPositionFromCoordinates(pathfinder.CurrentStartCoordinates);
            RecalculatePath(false);
        }
    }

    private void ReturnToStart()
    {
        pathfinder.SetRandomStartPosition();
        currentTarget = pathfinder.SetRandomDestinationPosition();
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
        bacteriaSpawner.enemyInfectedTarget -= OtherEnemyInfectedTarget;
        gameObject.SetActive(false);
                
        if (CheckForPatientToInfect(position))
        {
            InfectionRate.Instance.InfectPatientAtLocation(position);
            enemy.StealGold();
        }

        bacteriaSpawner.EnemyReachedTarget(position);
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

    private void TimerStopped()
    {
        StopAllCoroutines();        
    }

    private void TimerResumed()
    {
        if (gameObject.activeInHierarchy)
        {
            RecalculatePath(false);
        }        
    }
}
