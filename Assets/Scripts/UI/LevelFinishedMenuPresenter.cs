using UnityEngine;

public class LevelFinishedMenuPresenter : MonoBehaviour
{
    private Canvas levelFinishedCanvas;

    public int currentLevel;

    private void Awake()
    {
        LevelManager.Instance.onLevelFinished += ShowLevelFinishedMenu;
        levelFinishedCanvas = GetComponent<Canvas>();
        levelFinishedCanvas.enabled = false;
    }

    private void ShowLevelFinishedMenu(int currentLevelFinished)
    {
        currentLevel = currentLevelFinished;
        levelFinishedCanvas.enabled = true;
    }

    public void LoadLevel()
    {
        LevelManager.Instance.onLevelFinished -= ShowLevelFinishedMenu;
        LevelManager.Instance.LoadLevel(currentLevel + 1);
        levelFinishedCanvas.enabled = false;
    }

    public void LoadMainMenu()
    {
        LevelManager.Instance.onLevelFinished -= ShowLevelFinishedMenu;
        LevelManager.Instance.LoadMainMenu();
        levelFinishedCanvas.enabled = false;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.onLevelFinished -= ShowLevelFinishedMenu;
    }
}