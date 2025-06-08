using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Drawing.Text;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class Generate : MonoBehaviour
{
    [Header("鏡頭")]
    public CinemachineTargetGroup targetGroup;
    public CinemachineVirtualCamera cinemachine;

    [Header("初始化腳色物件")]
    public GameObject[] playerPrefabs;
    public GameObject PlayerTag;
    public Transform P1;
    public Transform P2;
    private Character p1Character;
    private Character p2Character;
    private GameObject p1, p2, t1, t2;

    [Header("血條控制")]
    public GameObject p1FrameRed;
    public GameObject p1FrameGreen;
    public GameObject p2FrameRed;
    public GameObject p2FrameGreen;
    private Image greenImage1, greenImage2, redImage1, redImage2;

    [Header("藍量控制")]
    public GameObject p1Mana;
    public GameObject p2Mana;
    private float p1ManaPercent = 1, p2ManaPercent = 1;
    private Image p1ManaImage, p2ManaImage;

    [Header("精力條控制")]
    public GameObject p1Power;
    public GameObject p2Power;
    private float p1PowerPercent = 1, p2PowerPercent = 1;
    private Image p1PowerImage, p2PowerImage;

    [Header("頭像")]
    public GameObject[] p1Face;
    public GameObject[] p2Face;

    [Header("時間")]
    public TMP_Text timeTXT;
    public float startTime;
    public float remainTime;
    public float wraningTime;
    bool isRunning;

    [Header("遊戲狀態")]
    public TMP_Text gameState;
    public GameObject gameOverBoard;
    public GameObject continueBotton;
    private bool isEnd;
    private void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        int player1 = PlayerPrefs.GetInt("Player1");
        int player2 = PlayerPrefs.GetInt("Player2");
        //創建角色實例
        p1 = Instantiate(playerPrefabs[player1], P1.position, P1.rotation);
        p2 = Instantiate(playerPrefabs[player2], P2.position, P2.rotation);
        if (p1 != null && p2 != null)
        {
            p1.GetComponent<WhoControl>().playerType = PlayerType.player1;
            p2.GetComponent<WhoControl>().playerType = PlayerType.player2;
            p2.GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
            
            p1.tag = "player1";
            p2.tag = "player2";

            p1.GetComponent<WhoControl>().Enable();
            p1.GetComponent<AIInputReader>().enabled = false;

            if (PlayerPrefs.GetInt("is all player") == 1)
            {
                // 第二位是玩家
                p2.GetComponent<AIInputReader>().enabled = false;
                p2.GetComponent<WhoControl>().Enable();
            }
            else
            {
                // 第二位是 AI
                p2.GetComponent<WhoControl>().enabled = false;
                p2.GetComponent<AIInputReader>().Enable();
            }

            greenImage1 = p1FrameGreen.GetComponent<Image>();
            greenImage2 = p2FrameGreen.GetComponent<Image>();
            redImage1 = p1FrameRed.GetComponent<Image>();
            redImage2 = p2FrameRed.GetComponent<Image>();
            p1PowerImage = p1Power.GetComponent<Image>();
            p2PowerImage = p2Power.GetComponent<Image>();
            p1ManaImage = p1Mana.GetComponent<Image>();
            p2ManaImage = p2Mana.GetComponent<Image>();

            p1.GetComponent<Character>().OnTakeDamage += P1HealthChange;
            p2.GetComponent<Character>().OnTakeDamage += P2HealthChange;

            p1Face[player1].SetActive(true);
            p2Face[player2].SetActive(true);

            p1Character = p1.GetComponent<Character>();
            p2Character = p2.GetComponent<Character>();
        }
        else
        {
            Debug.Log("not find player");
        }
        //處理tag
        if (PlayerTag != null)
        {
            t1 = Instantiate(PlayerTag, p1.transform.Find("nameTagPosition").position, Quaternion.identity);
            t2 = Instantiate(PlayerTag, p2.transform.Find("nameTagPosition").position, Quaternion.identity);

            PlayerTag pt1, pt2;
            pt1 = t1.GetComponent<PlayerTag>();
            pt2 = t2.GetComponent<PlayerTag>();

            pt1.player = p1.transform.Find("nameTagPosition");
            pt2.player = p2.transform.Find("nameTagPosition");

            pt1.nameText.text = "P1";
            if (PlayerPrefs.GetInt("is all player") == 1) pt2.nameText.text = "P2";
            else pt2.nameText.text = "AI";
        }
        else
        {
            Debug.Log("not find tag");
        }
        //處理鏡頭
        if (cinemachine != null)
        {
            cinemachine.Follow = targetGroup.transform;
            cinemachine.LookAt = targetGroup.transform;
        }
        else
        {
            Debug.Log("not find cinemachine");
        }

        if (targetGroup != null)
        {
            targetGroup.m_Targets = new CinemachineTargetGroup.Target[]
            {
                new CinemachineTargetGroup.Target
                {
                    target = p1.transform,
                    weight = 0.1f,
                    radius = 0.5f
                },
                new CinemachineTargetGroup.Target
                {
                    target = p2.transform,
                    weight = 0.1f,
                    radius = 0.5f
                }
            };
        }
        else
        {
            Debug.Log("not find targetGroup");
        }
        //時間相關
        isRunning = true;
        remainTime = startTime;

        isEnd = false;
    }

    private void Update()
    {
        TimeRelateFrameChange();
        countDown();
        detectGameOver();
    }
    //UI數值變化相關
    private void TimeRelateFrameChange()
    {
        if (redImage1.fillAmount > greenImage1.fillAmount) redImage1.fillAmount -= Time.deltaTime;
        if (redImage2.fillAmount > greenImage2.fillAmount) redImage2.fillAmount -= Time.deltaTime;
        //更新
        p1PowerPercent = p1Character.currentPower / p1Character.maxPower;
        p2PowerPercent = p2Character.currentPower / p2Character.maxPower;
        p1ManaPercent = p1Character.currentMana / p1Character.maxMana;
        p2ManaPercent = p2Character.currentMana / p2Character.maxMana;
        //UI變化
        p1PowerImage.fillAmount = Mathf.MoveTowards(p1PowerImage.fillAmount, p1PowerPercent, 2f * Time.deltaTime);
        p2PowerImage.fillAmount = Mathf.MoveTowards(p2PowerImage.fillAmount, p2PowerPercent, 2f * Time.deltaTime);
        p1ManaImage.fillAmount = Mathf.MoveTowards(p1ManaImage.fillAmount, p1ManaPercent, 2f * Time.deltaTime);
        p2ManaImage.fillAmount = Mathf.MoveTowards(p2ManaImage.fillAmount, p2ManaPercent, 2f * Time.deltaTime);
    }
    public void P1HealthChange(Transform t)
    {
        if (greenImage1 != null)
        {
            float healthPercent = p1Character.currentHealth / p1Character.maxHealth;
            greenImage1.fillAmount = healthPercent;
        }
    }
    public void P2HealthChange(Transform t)
    {
        if (greenImage2 != null)
        {
            float healthPercent = p2Character.currentHealth / p2Character.maxHealth;
            greenImage2.fillAmount = healthPercent;
        }
    }
    //時間相關
    private void countDown()
    {
        if(isRunning)
        {

            remainTime -= Time.deltaTime;
            int second = Mathf.CeilToInt(remainTime);
            timeTXT.text = string.Format("{0:000}", second >= 0 ? second : 0);
            if (second <= 0) 
            {
                isRunning = false;
                timeTXT.color = Color.white;
            }
            else if (remainTime <= wraningTime)
            {
                float speed = 5f;
                float alpha = Mathf.PingPong(Time.time * speed, 1f); // 速度 *5，越大閃越快
                timeTXT.color = new Color(1f, 0f, 0f, alpha); // 紅色閃爍
            }
            else
            {
                timeTXT.color = Color.white;
            }
        }
    }
    //偵測遊戲是否結束
    private void detectGameOver()
    {
        Character P1RemainHeaith = p1.GetComponent<Character>();
        Character P2RemainHeaith = p2.GetComponent<Character>();
        if(isEnd == false)
        if (isRunning == false || P1RemainHeaith.currentHealth == 0 || P2RemainHeaith.currentHealth == 0)
        {
            gameOverBoard.SetActive(true);
            continueBotton.SetActive(true);

            remainTime = 0;
            timeTXT.color = Color.white;

            isEnd = true;

            if(P1RemainHeaith.currentHealth == P2RemainHeaith.currentHealth)
            {
                gameState.text = "Draw";
            }
            if(P1RemainHeaith.currentHealth > P2RemainHeaith.currentHealth)
            {
                gameState.text = "P1 Win";
            }
            if(P1RemainHeaith.currentHealth < P2RemainHeaith.currentHealth)
            {
                if (PlayerPrefs.GetInt("is all player") == 0) gameState.text = "AI Win";
                else gameState.text = "P2 Win";
            }
        }
    }
    public void EndGame()//按鈕觸發
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Start");
    }
}
            
        
        