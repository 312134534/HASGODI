using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("暫停選單")]
    public GameObject pauseMenuUI;
    [Header("按鍵設定")]
    public GameObject ChangeBotton;
    private bool isPaused = false;
    private ActionControll inputActions;

    private void Awake()
    {
        inputActions = new ActionControll();
        inputActions.UI.Stop.performed += ctx => TogglePause();
    }
    private void OnEnable()
    {
        inputActions.UI.Enable();
    }

    private void OnDisable()
    {
        inputActions.UI.Disable();
    }
    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start"); 
    }
    public void Setting()
    {
        ChangeBotton.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void DisableSetting()
    {
        ChangeBotton.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
