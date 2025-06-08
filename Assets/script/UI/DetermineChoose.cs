using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DetermineChoose : MonoBehaviour
{
    private int chooseOrder = 0;
    private string controlP2;
    [Header("選角文字")]
    public TMP_Text text;

    [Header("立繪")]
    public GameObject suifuL;
    public GameObject suifuR;
    public GameObject hansL;
    public GameObject hansR;
    private void Start()
    {
        if (PlayerPrefs.GetInt("is all player") == 1) controlP2 = "Player 2 Choose";
        else controlP2 = "Choose AI Character";
    }
    private void Update()
    {
        if (chooseOrder >= 2)
        {
            SceneManager.LoadScene("map 1");
        }
    }

    //  這裡用滑鼠移入觸發（取代 OnSelect）
    public void ShowTechie(Role role)
    {
        Debug.Log("滑鼠移入：" + role);

        switch (role)
        {
            case Role.suifu:
                if (chooseOrder == 0)
                {
                    hansL.SetActive(false);
                    suifuL.SetActive(true);
                }
                else
                {
                    hansR.SetActive(false);
                    suifuR.SetActive(true);
                }
                break;
            case Role.hans:
                if (chooseOrder == 0)
                {
                    suifuL.SetActive(false);
                    hansL.SetActive(true);
                }
                else
                {
                    suifuR.SetActive(false);
                    hansR.SetActive(true);
                }
                break;
        }
    }
    public void ChooseSuifu()
    {
        switch (chooseOrder)
        {
            case 0:
                PlayerPrefs.SetInt("Player1", (int)Role.suifu);
                break;
            case 1:
                PlayerPrefs.SetInt("Player2", (int)Role.suifu);
                break;
        }
        chooseOrder++;
        text.text = controlP2;
        suifuR.SetActive(true);
    }

    public void ChooseHans()
    {
        switch (chooseOrder)
        {
            case 0:
                PlayerPrefs.SetInt("Player1", (int)Role.hans);
                break;
            case 1:
                PlayerPrefs.SetInt("Player2", (int)Role.hans);
                break;
        }
        chooseOrder++;
        text.text = controlP2;
        hansR.SetActive(true);
    }
    
}

