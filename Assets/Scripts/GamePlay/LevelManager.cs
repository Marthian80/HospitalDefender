using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private float sceneLoadDelay = 2f;

    public event Action onGameEnded;

    protected override void Awake()
    {
        base.Awake();        
    }

    public void LoadGameOver()
    {
        GameEnded(true);        
    }

    public void LoadLevelWon()
    {
        GameEnded(false);        
    }

    public void LoadGame()
    {
        AudioPlayer.Instance.PlayButtonClickClip();
        AudioPlayer.Instance.PlayGameMusic();
        SceneManager.LoadScene("LevelOne");
        Timer.Instance.ResetTimer();
        InfectionRate.Instance.ResetInfectedPatients();
        Bank.Instance.ResetToStartingBalance();
    }

    public void LoadMainMenu()
    {
        AudioPlayer.Instance.PlayButtonClickClip();
        AudioPlayer.Instance.PlayMenuMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadInstructions()
    {
        AudioPlayer.Instance.PlayButtonClickClip();
        AudioPlayer.Instance.PlayMenuMusic();
        SceneManager.LoadScene("Instructions");
    }

    public void QuitGame()
    {
        AudioPlayer.Instance.PlayButtonClickClip();
        Application.Quit();
    }

    private void GameEnded(bool gameLost)
    {
        AudioPlayer.Instance.PlayMenuMusic();

        if (onGameEnded != null)
        {
            onGameEnded();
        }
        if (gameLost)
        {
            AudioPlayer.Instance.PlayGameEndClip(gameLost);            
            StartCoroutine(WaitAndLoad("GameOver", sceneLoadDelay));
        }
        else
        {
            AudioPlayer.Instance.PlayGameEndClip(!gameLost);
            StartCoroutine(WaitAndLoad("GameWon", sceneLoadDelay));
        }
    }

    private IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
