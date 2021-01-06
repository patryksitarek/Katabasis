using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    public Image background;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject levelCompleteMenu;
    public LevelMusic levelMusic;

    public bool isPaused { private set; get; } = false;
    private Fade fade;

    private void Start()
    {
        fade = background.GetComponent<Fade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) UnpauseGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) UnpauseGame();
        else isPaused = true;
        background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        fade.FadeIn(1);
        Time.timeScale = 1;
    }

    private IEnumerator ToggleMenu(GameObject menu, float time, bool state)
    {
        yield return new WaitForSeconds(time);

        menu.SetActive(state);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlayMusic(levelMusic.levelMusic);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnGameOver()
    {
        fade.FadeOut(.4f);
        StartCoroutine(ToggleMenu(gameOverMenu, 1f, true));
    }

    public void OnLevelComplete()
    {
        fade.FadeOut(.4f);
        StartCoroutine(ToggleMenu(levelCompleteMenu, 1f, true));
    }
}
