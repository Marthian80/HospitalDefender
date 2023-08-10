using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private BuildMenuPresenter buildMenu;

    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    [SerializeField] private bool containsPatient;
    public bool ContainsPatient { get { return containsPatient; } }

    private Gridmanager gridManager;
    private Pathfinding pathFinder;
    private Vector2Int coordinates = new Vector2Int();
    private bool buildTowerSelected = false;

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();  
        pathFinder = FindObjectOfType<Pathfinding>();        
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);

                if (containsPatient)    
                {
                    gridManager.SetNodeAsContainingPatient(coordinates);
                }
            }
            else
            {
                buildMenu.onBuildTowerSelectionChanged += UpdateBuildSelection;
            }
        }
    }    

    private void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates) && buildTowerSelected)
        {
            bool isSuccessfull = towerPrefab.CreateTower(towerPrefab, transform.position);
            if (isSuccessfull)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
    }

    private void UpdateBuildSelection(bool selectionState)
    {
        buildTowerSelected = selectionState;
    }
}
