using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // �b�s�边�������
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �b���]�᪺�C�����h�X
    Application.Quit();
#endif
    }
}
