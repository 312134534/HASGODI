using System.Collections;
using UnityEngine;

public class AfterimageSpawner : MonoBehaviour
{
    [Header("殘影元件")]
    [SerializeField] private SpriteRenderer spriteRenderer;     // 拖角色的 SpriteRenderer
    [SerializeField] private GameObject afterimagePrefab;       // 殘影的預製體（Prefab）
    [SerializeField] private Material afterimageMaterial;       // 使用的 Shader 材質
    [SerializeField] private Color afterimageColor = Color.white; // 角色殘影顏色（可在 Inspector 設定）

    [Header("殘影參數")]
    [SerializeField] private float spawnInterval = 0.05f;       // 每隔幾秒產生一個
    [SerializeField] private float afterimageDuration = 0.3f;   // 殘影存在多久

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

        // 為每個殘影創建獨立材質，防止材質共用導致顏色被覆蓋
        Material newMat = new Material(afterimageMaterial);
        newMat.color = afterimageColor;
        sr.material = newMat;

        // 可選：直接設置 color，也能混合使用
        sr.color = afterimageColor;

        Destroy(afterimage, afterimageDuration);
    }
}
