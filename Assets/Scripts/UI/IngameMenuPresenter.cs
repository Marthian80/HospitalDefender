using UnityEngine;

public class IngameMenuPresenter : MonoBehaviour
{
    [SerializeField] private int currentLevel;

    private Canvas ingameMenuCanvas;    

    private void Awake()
    {
        LevelManager.Instance.onShowIngameMenu += ShowIngameMenu;
        ingameMenuCanvas = GetComponent<Canvas>();
        ingameMenuCanvas.enabled = false;
    }    

    public void ShowIngameMenu()
    {
        if (!ingameMenuCanvas.enabled)
        {
            ingameMenuCanvas.enabled = true;
            Timer.Instance.StopTimer();
        }        
    }

    public void ResumeGame()
    {
        ingameMenuCanvas.enabled = false;
        Timer.Instance.ResumeTimer();
    }

    public void LoadMainMenu()
    {
        LevelManager.Instance.onShowIngameMenu -= ShowIngameMenu;
        LevelManager.Instance.LoadMainMenu();
        ingameMenuCanvas.enabled = false;
    }

    public void RestartLevel()
    {
        LevelManager.Instance.onShowIngameMenu -= ShowIngameMenu;
        LevelManager.Instance.FastLoadLevel(currentLevel);
        ingameMenuCanvas.enabled = false;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.onShowIngameMenu -= ShowIngameMenu;
    }
}