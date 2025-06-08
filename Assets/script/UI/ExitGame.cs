using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // 在編輯器中停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在打包後的遊戲中退出
    Application.Quit();
#endif
    }
}
