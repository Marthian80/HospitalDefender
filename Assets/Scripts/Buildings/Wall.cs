using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private BuildMenuPresenter buildMenu;
    [SerializeField] private Poster posterPrefab;

    private SpriteRenderer spriteRendererPoster;
    private Flash flash;
    private bool buildPosterSeleted = false;
    private bool hasPosterBuild = false;

    private const string PosterBuildPreview = "PosterBuildPreview";

    private void Awake()
    {
        spriteRendererPoster = transform.Find(PosterBuildPreview).GetComponent<SpriteRenderer>();
        flash = GetComponent<Flash>();        
    }

    private void Start()
    {
        buildMenu.onBuildingSelectionChanged += UpdateBuildSelection;
    }

    private void OnMouseOver()
    {
        if (buildPosterSeleted && Bank.Instance.CurrentBalance >= posterPrefab.Cost)
        {
            ShowBuildSpacePreview(spriteRendererPoster);
        }
    }

    private void OnMouseExit()
    {
        if (spriteRendererPoster.enabled)
        {
            StopShowBuildSpacePreview();
        }
    }

    private void OnMouseDown()
    {
        if (!hasPosterBuild && buildPosterSeleted && Bank.Instance.CurrentBalance >= posterPrefab.Cost)
        {
            posterPrefab.CreatePoster(posterPrefab, transform.position);
            hasPosterBuild = false;
            StopShowBuildSpacePreview();
        }
    }

    private void UpdateBuildSelection(int? selectionState)
    {
        buildPosterSeleted = false;        

        if (selectionState == (int)GlobalEnums.Buildings.Poster)
        {
            buildPosterSeleted = true;
        }
        else
        {
            buildPosterSeleted = false;
            if (selectionState == null)
            {
                StopShowBuildSpacePreview();
            }
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
        spriteRendererPoster.color = new Color(spriteRendererPoster.color.r, spriteRendererPoster.color.b, spriteRendererPoster.color.g, 1f);
        spriteRendererPoster.enabled = false;
    }
}
