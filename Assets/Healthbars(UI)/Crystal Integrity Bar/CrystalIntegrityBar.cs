using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrystalIntegrityBar : MonoBehaviour
{
    public RectTransform fillRect;
    public RectTransform maskRect;

    public int maxHealth = 100;
    private int currentHealth;

    private float fullWidth;

    void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(InitializeBar());
    }

   


    private IEnumerator InitializeBar()
    {
        yield return new WaitForEndOfFrame();

        fullWidth = maskRect.rect.width;
        UpdateBar();
        Debug.Log($"[CrystalBar] INIT: fullWidth = {fullWidth}");
        UpdateBar();
    }

    private Coroutine currentAnim;

    public void SetHealth(int value, bool animate = false)
    {
        int clamped = Mathf.Clamp(value, 0, maxHealth);
        Debug.Log($"[CrystalBar] SetHealth({clamped}) called");

        if (animate)
        {
            if (currentAnim != null)
                StopCoroutine(currentAnim);

            currentAnim = StartCoroutine(AnimateFill(clamped));
        }
        else
        {
            currentHealth = clamped;
            UpdateBar();
        }
    }



    public void TakeDamage(int amount)
    {
        SetHealth(currentHealth - amount, animate: true);
    }

    public void Repair(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    public void EnableBar(bool enabled)
    {
        gameObject.SetActive(enabled);

        if (enabled)
        {
            StartCoroutine(InitializeBar());
        }
    }

    private void UpdateBar()
    {
        if (fillRect != null)
        {
            float percent = (float)currentHealth / maxHealth;
            float width = fullWidth * percent;
            Debug.Log($"[CrystalBar] UpdateBar: width = {width}");

            fillRect.sizeDelta = new Vector2(width, fillRect.sizeDelta.y);
        }
    }
   

    private IEnumerator AnimateFill(int targetHealth)
    {
        float duration = 0.65f;
        float startPercent = (float)currentHealth / maxHealth;
        float endPercent = (float)targetHealth / maxHealth;
        float t = 0f;

        currentHealth = targetHealth;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float percent = Mathf.Lerp(startPercent, endPercent, t);
            float width = fullWidth * percent;

            fillRect.sizeDelta = new Vector2(width, fillRect.sizeDelta.y);
            yield return null;
        }

        fillRect.sizeDelta = new Vector2(fullWidth * endPercent, fillRect.sizeDelta.y);
    }

}
