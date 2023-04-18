using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CanvasGroup PlayerCanvasGroup;
    public CanvasGroup PauseCanvasGroup;

    private bool _isPaused = false;

    public void RestartLevel()
    {
        //add transition here
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause()
    {
        if (_isPaused) ResumeGame();
        else PauseGame();
    }

    public void BackToMenu(Animation animation)
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(AnimationHelper.AnimateButton(animation, () => SceneManager.LoadScene("MainMenu")));
    }

    public void ResumeGame(Animation animation)
    {
        Time.timeScale = 1f;
        StartCoroutine(AnimationHelper.AnimateButton(animation, () => ResumeGame()));
    }

    private void PauseGame()
    {
        _isPaused = true;
        PlayerCanvasGroup.alpha = 0f;
        PauseCanvasGroup.alpha = 0.8f;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame()
    {
        _isPaused = false;
        PlayerCanvasGroup.alpha = 1f;
        PauseCanvasGroup.alpha = 0f;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsPaused
    {
        get { return _isPaused; }
    }
}
