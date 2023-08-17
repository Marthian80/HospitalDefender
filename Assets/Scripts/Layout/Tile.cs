using Unity.VisualScripting.Dependencies.NCalc;
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
    private AudioPlayer audioPlayer;        
    private Vector2Int coordinates = new Vector2Int();
    private bool buildTowerSelected = false;

    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();  
        pathFinder = FindObjectOfType<Pathfinding>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isWalkable)
            {
                gridManager.BlockNode(coordinates);                
            }
            if (isPlaceable)
            {
                buildMenu.onBuildTowerSelectionChanged += UpdateBuildSelection;
            }

            if (containsPatient)
            {
                gridManager.SetNodeAsContainingPatient(coordinates);
            }
        }
    }    

    private void OnMouseDown()
    {
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTowerSelected)
        {
            bool isSuccessfull = towerPrefab.CreateTower(towerPrefab, transform.position, audioPlayer);
            if (isSuccessfull)
            {
                isPlaceable = false;
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyReceivers();
            }
        }
        else
        {
            Debug.Log($"Is walkable {gridManager.GetNode(coordinates).isWalkable}");
            Debug.Log($"Path blocked {!pathFinder.WillBlockPath(coordinates)}");
        }
    }

    private void UpdateBuildSelection(bool selectionState)
    {
        buildTowerSelected = selectionState;
    }
}
