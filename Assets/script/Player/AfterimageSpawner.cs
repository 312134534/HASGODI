using System.Collections;
using UnityEngine;

public class AfterimageSpawner : MonoBehaviour
{
    [Header("�ݼv����")]
    [SerializeField] private SpriteRenderer spriteRenderer;     // �쨤�⪺ SpriteRenderer
    [SerializeField] private GameObject afterimagePrefab;       // �ݼv���w�s��]Prefab�^
    [SerializeField] private Material afterimageMaterial;       // �ϥΪ� Shader ����
    [SerializeField] private Color afterimageColor = Color.white; // ����ݼv�C��]�i�b Inspector �]�w�^

    [Header("�ݼv�Ѽ�")]
    [SerializeField] private float spawnInterval = 0.05f;       // �C�j�X���ͤ@��
    [SerializeField] private float afterimageDuration = 0.3f;   // �ݼv�s�b�h�[

    private Coroutine spawnCoroutine;

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnAfterimages());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnAfterimages()
    {
        while (true)
        {
            CreateAfterimage();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void CreateAfterimage()
    {
        GameObject afterimage = Instantiate(afterimagePrefab, transform.position, Quaternion.identity);

        SpriteRenderer sr = afterimage.GetComponent<SpriteRenderer>();
        sr.sprite = spriteRenderer.sprite;
        sr.flipX = spriteRenderer.flipX;
        sr.transform.localScale = transform.localScale;

        // ���C�Ӵݼv�ЫؿW�ߧ���A�������@�ξɭP�C��Q�л\
        Material newMat = new Material(afterimageMaterial);
        newMat.color = afterimageColor;
        sr.material = newMat;

        // �i��G�����]�m color�A�]��V�X�ϥ�
        sr.color = afterimageColor;

        Destroy(afterimage, afterimageDuration);
    }
}
