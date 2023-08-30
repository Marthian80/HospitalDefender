using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private int cost = 150;
    public int Cost { get { return cost; } }

    [SerializeField][Range(0f, 5f)] float speed = 1f;


    private List<Node> path = new List<Node>();
    private Gridmanager gridManager;
    private Pathfinding pathfinder;    
    private SpriteRenderer spriteRendererTankHorizontal;
    private SpriteRenderer spriteRendererTankVertical;

    private bool tankRotatedHorizontal = true;

    private const string horizontalTankSprite = "HorizontalTank";
    private const string verticalTankSprite = "VerticalTank";

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();
        pathfinder = FindObjectOfType<Pathfinding>();
        
        TankActivator.Instance.onTankBuildAtlocation += StartNewPath;
        //Activate event listener
        EventManager.StartListening("RecalculatePath", RecalculatePath);
    }

    private void StartNewPath(Vector2Int startCoordinates)
    {
        if (gameObject.activeInHierarchy)
        {
            pathfinder.SetStartPositionOnTheseCoordinates(startCoordinates);
            pathfinder.SetRandomTankDestinationPosition();
            transform.position = gridManager.GetPositionFromCoordinates(pathfinder.CurrentStartCoordinates);
            RecalculatePath(false);
        }
    }

    private void FinishPath(Vector2 position)
    {   
        StartNewPath(new Vector2Int((int)position.x, (int)position.y));
    }

    private void RecalculatePath(bool notUsed)
    {    
        Vector2Int coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPathRoutine());
    }

    private IEnumerator FollowPathRoutine()
    {
        Vector2 endPosition = transform.position;
        Vector2 startPosition = transform.position;

        for (int i = 1; i < path.Count; i++)
        {
            startPosition = transform.position;
            endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            CheckForRotateTank(startPosition, endPosition);
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

    private void CheckForRotateTank(Vector2 start, Vector2 end)
    {
        spriteRendererTankHorizontal = transform.Find(horizontalTankSprite).GetComponent<SpriteRenderer>();
        spriteRendererTankVertical = transform.Find(verticalTankSprite).GetComponent<SpriteRenderer>();

        if (start.y != end.y && tankRotatedHorizontal)
        {
            tankRotatedHorizontal = false;
            spriteRendererTankHorizontal.enabled = false;
            spriteRendererTankVertical.enabled = true;
            Debug.Log("Tank now goes vertical");
        }
        else if (start.x != end.x && !tankRotatedHorizontal)
        {
            tankRotatedHorizontal = true;
            spriteRendererTankHorizontal.enabled = true;
            spriteRendererTankVertical.enabled = false;
            Debug.Log("Tank now goes horizonal");
        }
    }
}
