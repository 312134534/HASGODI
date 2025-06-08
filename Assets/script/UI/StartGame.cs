using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [Header("«ö¶s")]
    public GameObject pvp;
    public GameObject pvc;
    public GameObject start;
    public GameObject exit;
    public void OnStartButtonPressed()
    {
        pvp.SetActive(true);
        pvc.SetActive(true);
        exit.SetActive(false);
        start.SetActive(false);
    }

    public void OnChoosePlayer()
    {
        PlayerPrefs.SetInt("is all player", 1);
        Debug.Log("pvp");
        SceneManager.LoadScene("Choose Character");
    }

    public void OnChooseAI()
    {
        PlayerPrefs.SetInt("is all player", 0);
        Debug.Log("pvc");
        SceneManager.LoadScene("Choose Character");
    }
}
