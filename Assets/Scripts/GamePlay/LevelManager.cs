using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private float sceneLoadDelay = 0.25f;
    [SerializeField] private int numberOfLevelsInGame = 3;

    public event Action onGameEnded;
    public event Action<int> onLevelFinished;
    public event Action onShowIngameMenu;



    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (onShowIngameMenu != null)
            {
                onShowIngameMenu();
            }
        }
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode arg1)
    {        
        if (loadedScene.name.Contains("Level"))
        {
            ResetGameData();
            if (loadedScene.name == "Level1")
            {
                Timer.Instance.SetTime(GlobalGameData.timeLevelOne);
            }
            else if (loadedScene.name == "Level2")
            {
                Timer.Instance.SetTime(GlobalGameData.timeLevelTwo);
            }
            else if (loadedScene.name == "Level3")
            {
                Timer.Instance.SetTime(GlobalGameData.timeLevelThree);
            }
        }
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
        SceneManager.LoadScene("Level1");
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

    public void LoadLevel(int sceneIndex)
    {       
        StartCoroutine(WaitAndLoad("Level" + sceneIndex, sceneLoadDelay));       
    }

    public void FastLoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene("Level" + sceneIndex);
    }

    public void FinishedLevel()
    {        
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == numberOfLevelsInGame)
        {
            GameEnded(false);
        }
        else
        {
            if (onLevelFinished != null)
            {
                onLevelFinished(currentSceneIndex);
            }
            else
            {
                LoadMainMenu();
            }
        }
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

    private void ResetGameData()
    {
        Timer.Instance.ResetTimer();
        InfectionRate.Instance.ResetInfectedPatients();
        Bank.Instance.ResetToStartingBalance();
    }
}
