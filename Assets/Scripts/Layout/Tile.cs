using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;      
    [SerializeField] private Tank tankPrefab;
    [SerializeField] private Poster posterPrefab;
    [SerializeField] private BuildMenuPresenter buildMenu;

    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }    

    [SerializeField] private bool isWalkable;
    public bool IsWalkable { get { return isWalkable; } }

    [SerializeField] private bool containsPatient;
    public bool ContainsPatient { get { return containsPatient; } }

    public bool SlowEnemies { get; private set; }

    private Gridmanager gridManager;
    private Pathfinding pathFinder;    
    private SpriteRenderer spriteRendererTower;    
    private SpriteRenderer spriteRendererTank;

    private Flash flash;
    private Vector2Int coordinates = new Vector2Int();
    private bool buildTowerSelected = false;      
    private bool buildTankSelected = false;
    
    private const string TowerBuildPreview = "TowerBuildPreview";    
    private const string TankBuildPreview = "TankBuildPreview";


    private void Awake()
    {
        gridManager = FindObjectOfType<Gridmanager>();  
        pathFinder = FindObjectOfType<Pathfinding>();        
        spriteRendererTower = transform.Find(TowerBuildPreview).GetComponent<SpriteRenderer>();        
        spriteRendererTank = transform.Find(TankBuildPreview).GetComponent<SpriteRenderer>();
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
            buildMenu.onBuildingSelectionChanged += UpdateBuildSelection;
        }
        if (isWalkable)
        {
            posterPrefab.onPosterBuildAtlocation += ApplySlowness;
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
        else if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTankSelected && Bank.Instance.CurrentBalance >= tankPrefab.Cost)
        {
            TankActivator.Instance.ActivateTank(transform.position);
            StopShowBuildSpacePreview();
        }
    }

    private void OnMouseOver()
    {
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTowerSelected && Bank.Instance.CurrentBalance >= towerPrefab.Cost)
        {            
            ShowBuildSpacePreview(spriteRendererTower);
        }
        if (isPlaceable && !pathFinder.WillBlockPath(coordinates) && buildTankSelected && Bank.Instance.CurrentBalance >= tankPrefab.Cost)
        {
            ShowBuildSpacePreview(spriteRendererTank);
        }
    }

    private void OnMouseExit()
    {
        if(spriteRendererTower.enabled || spriteRendererTank.enabled)
        {
            StopShowBuildSpacePreview();
        }
    }    

    private void UpdateBuildSelection(int? selectionState)
    {        
        buildTowerSelected = false;
        buildTankSelected = false;

        switch (selectionState)
        {            
            case (int)GlobalEnums.Buildings.Tower:
                buildTowerSelected = true;
                break;
            case (int)GlobalEnums.Buildings.Tank:
                buildTankSelected = true;
                break;
            default:
                StopShowBuildSpacePreview();
                break;
        }
    }  
    
    private void ShowBuildSpacePreview(SpriteRenderer spriteRenderer)
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
        spriteRendererTower.color = new Color(spriteRendererTower.color.r, spriteRendererTower.color.b, spriteRendererTower.color.g, 1f);
        spriteRendererTower.enabled = false;
        spriteRendererTank.color = new Color(spriteRendererTank.color.r, spriteRendererTank.color.b, spriteRendererTank.color.g, 1f);
        spriteRendererTank.enabled = false;

    }

    private void ApplySlowness(Vector2 slownessEffectCoordinates)
    {
        if (coordinates.x == (int)slownessEffectCoordinates.x && coordinates.y == (int)slownessEffectCoordinates.y)
        {
            Debug.Log($"Slow set on X: {coordinates.x}, Y {coordinates.y}");
            SlowEnemies = true;
        }
    }
}