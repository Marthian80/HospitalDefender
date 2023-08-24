using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private BuildMenuPresenter buildMenu;

    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    [SerializeField] private bool isWalkable;
    public bool IsWalkable { get { return isWalkable; } }

    [SerializeField] private bool containsPatient;
    public bool ContainsPatient { get { return containsPatient; } }
    
    private Gridmanager gridManager;
    private Pathfinding pathFinder;    
    private SpriteRenderer spriteRenderer;
    private Flash flash;
    private Vector2Int coordinates = new Vector2Int();
    private bool buildTowerSelected = false;

    private const string TowerBuildPreview = "TowerBuildPreview";

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();  
        pathFinder = FindObjectOfType<Pathfinding>();        
        spriteRenderer = transform.Find(TowerBuildPreview).GetComponent<SpriteRenderer>();
        flash = GetComponent<Flash>();

        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isWalkable)
            {
                gridManager.BlockNode(coordinates);
            }
            if (containsPatient)
            {
                gridManager.SetNodeAsContainingPatient(coordinates);
            }
        }
    }

    private void Start()
    {
        if (isPlaceable)
        {
            buildMenu.onBuildTowerSelectionChanged += UpdateBuildSelection;
        }
    }    

    private void OnMouseDown()
    {
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTowerSelected && Bank.Instance.CurrentBalance >= towerPrefab.Cost)
        {
            towerPrefab.CreateTower(towerPrefab, transform.position);
            isPlaceable = false;
            gridManager.BlockNode(coordinates);
            pathFinder.NotifyReceivers();
            StopShowBuildSpacePreview();
        }
        else
        {
            Debug.Log($"Is walkable {gridManager.GetNode(coordinates).isWalkable}");
            Debug.Log($"Path blocked {!pathFinder.WillBlockPath(coordinates)}");
        }
    }

    private void OnMouseOver()
    {
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTowerSelected && Bank.Instance.CurrentBalance >= towerPrefab.Cost)
        {            
            ShowBuildSpacePreview();
        }
    }

    private void OnMouseExit()
    {
        if(spriteRenderer.enabled)
        {
            StopShowBuildSpacePreview();
        }
    }    

    private void UpdateBuildSelection(bool selectionState)
    {
        buildTowerSelected = selectionState;
        if (!selectionState)
        {
            StopShowBuildSpacePreview();
        }
    }  
    
    private void ShowBuildSpacePreview()
    {
        if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
            StartCoroutine(flash.SlowFlashRoutine(spriteRenderer));
        }
    }

    private void StopShowBuildSpacePreview()
    {
        StopAllCoroutines();        
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, 1f);
        spriteRenderer.enabled = false;
    }
}
