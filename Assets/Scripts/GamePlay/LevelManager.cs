using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float sceneLoadDelay = 2f;

    private AudioPlayer audioPlayer;

    public event Action onGameEnded;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
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
        audioPlayer.PlayGameMusic();
        SceneManager.LoadScene("LevelOne");
    }

    public void LoadMainMenu()
    {
        audioPlayer.PlayMenuMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadInstructions()
    {
        audioPlayer.PlayMenuMusic();
        SceneManager.LoadScene("Instructions");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameEnded(bool gameLost)
    {
        audioPlayer.PlayMenuMusic();

        if (onGameEnded != null)
        {
            onGameEnded();
        }
        if (gameLost)
        {
            audioPlayer.PlayGameEndClip(gameLost);            
            StartCoroutine(WaitAndLoad("GameOver", sceneLoadDelay));
        }
        else
        {
            audioPlayer.PlayGameEndClip(!gameLost);
            StartCoroutine(WaitAndLoad("GameWon", sceneLoadDelay));
        }
    }

    IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
