using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PlayerTag : MonoBehaviour
{
    
    public TextMeshProUGUI nameText;
    public Transform player;

    private Camera cam;
    public float floatSpeed = 3f;
    public float floatHeight = 0.2f;
    private float noiseOffset;
    void Awake()
    {
        cam = Camera.main;
        noiseOffset = Random.Range(0f, 100f);
    }

    void LateUpdate()
    {
        if (cam != null && player != null)
        {
            float floatOffset = Mathf.Sin(Time.time * floatSpeed + noiseOffset) * floatHeight;
            Vector3 pos = player.position + new Vector3(0, floatOffset, 0);
            transform.position = pos;
            
            transform.rotation = Quaternion.identity;
        }
    }
}
