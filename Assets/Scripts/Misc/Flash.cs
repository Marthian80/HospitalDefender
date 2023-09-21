using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaultMatTime = 0.2f;
    [SerializeField] private float fadeTime = 0.75f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            defaultMat = spriteRenderer.material;
        }
    }

    public float GetRestoreDefaultMatTime()
    {
        return restoreDefaultMatTime;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }

    public IEnumerator SlowFlashRoutine(SpriteRenderer externalSpriteRenderer)
    {
        float elapsedTime;
        float startValue = externalSpriteRenderer.material.color.a;
        var color = externalSpriteRenderer.color;

        while (true)
        {
            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
                externalSpriteRenderer.color = new Color(color.r, color.b, color.g, newAlpha);
                yield return null;
            }
            elapsedTime = 0;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(0f, startValue, elapsedTime / fadeTime);
                externalSpriteRenderer.color = new Color(color.r, color.b, color.g, newAlpha);
                yield return null;
            }
        }
    }

    public IEnumerator SlowFadeOutRoutine(SpriteRenderer externalSpriteRenderer)
    {
        float elapsedTime = 0;
        float startValue = externalSpriteRenderer.material.color.a;
        var color = externalSpriteRenderer.color;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
            externalSpriteRenderer.color = new Color(color.r, color.b, color.g, newAlpha);
            yield return null;
        }
    }

    public IEnumerator SlowFadeInRoutine(SpriteRenderer externalSpriteRenderer)
    {
        float elapsedTime = 0;
        float startValue = externalSpriteRenderer.material.color.a;
        var color = externalSpriteRenderer.color;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, startValue, elapsedTime / fadeTime);
            externalSpriteRenderer.color = new Color(color.r, color.b, color.g, newAlpha);
            yield return null;
        }
    }
}
