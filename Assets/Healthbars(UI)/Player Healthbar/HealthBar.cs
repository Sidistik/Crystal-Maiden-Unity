using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public GameObject greenSegmentPrefab;
    public GameObject redOverlayPrefab;
    public GameObject darkOverlayPrefab;
    public Transform healthBarContainer;

    private List<Segment> segments = new List<Segment>();
    private int currentHealth = 5;
    private int maxHealth = 5;

    private const int maxBase = 5;
    private const int maxRed = 5;
    private const int maxDark = 5;
    private const int maxTotalHealth = maxBase + maxRed + maxDark;

    private class Segment
    {
        public GameObject baseGreen;
        public GameObject redOverlay;
        public GameObject darkOverlay;

        public Segment(GameObject green)
        {
            baseGreen = green;
            redOverlay = null;
            darkOverlay = null;
        }

        public void ClearOverlays()
        {
            if (redOverlay != null) GameObject.Destroy(redOverlay);
            if (darkOverlay != null) GameObject.Destroy(darkOverlay);
            redOverlay = null;
            darkOverlay = null;
        }

        public void AddOverlay(GameObject prefab, out GameObject overlayObj)
        {
            overlayObj = GameObject.Instantiate(prefab, baseGreen.transform);
            overlayObj.transform.localPosition = Vector3.zero;

            RectTransform rt = overlayObj.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public void SetActive(bool active)
        {
            baseGreen.SetActive(active);
        }
    }

    public void InitHealthBar()
    {
        foreach (Transform child in healthBarContainer)
            Destroy(child.gameObject);

        segments.Clear();

        for (int i = 0; i < maxBase; i++)
        {
            GameObject green = Instantiate(greenSegmentPrefab, healthBarContainer);
            green.transform.localPosition = Vector3.zero;
            segments.Add(new Segment(green));
        }

        SetHealth(5);
    }

    public void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxTotalHealth);
        maxHealth = currentHealth;
        UpdateVisuals();
        Debug.Log($"[HealthBar] SetHealth({currentHealth}) called\n{System.Environment.StackTrace}");



    }

    public void OnHealthIncreased()
    {
        if (currentHealth >= maxHealth) return;

        currentHealth++;
        UpdateVisuals(flickerNew: true);
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;

        float delay = 0.05f + (1f - Mathf.Clamp01(currentHealth / (float)maxHealth)) * 0.15f;

        for (int i = maxBase - 1; i >= 0; i--)
        {
            if (segments[i].darkOverlay != null)
            {
                StartCoroutine(FlickerThenDisable(segments[i].darkOverlay, true, delay));
                segments[i].darkOverlay = null;
                currentHealth--;
                return;
            }
        }

        for (int i = maxBase - 1; i >= 0; i--)
        {
            if (segments[i].redOverlay != null)
            {
                StartCoroutine(FlickerThenDisable(segments[i].redOverlay, true, delay));
                segments[i].redOverlay = null;
                currentHealth--;
                return;
            }
        }

        for (int i = maxBase - 1; i >= 0; i--)
        {
            if (segments[i].baseGreen.activeSelf)
            {
                StartCoroutine(FlickerThenDisable(segments[i].baseGreen, false, delay));
                currentHealth--;
                return;
            }
        }
    }

    private void UpdateVisuals(bool flickerNew = false)
    {
        foreach (var segment in segments)
        {
            segment.ClearOverlays();
            segment.SetActive(true);
        }

        int greenCount = Mathf.Min(currentHealth, maxBase);
        int redCount = Mathf.Clamp(currentHealth - maxBase, 0, maxRed);
        int darkCount = Mathf.Clamp(currentHealth - maxBase - maxRed, 0, maxDark);

        for (int i = maxBase - 1; i >= greenCount; i--)
        {
            segments[i].SetActive(false);
        }

        for (int i = 0; i < redCount; i++)
        {
            segments[i].AddOverlay(redOverlayPrefab, out segments[i].redOverlay);

            if (flickerNew && i == redCount - 1 && darkCount == 0)
                StartCoroutine(FlickerThenEnable(segments[i].redOverlay, true));
        }

        for (int i = 0; i < darkCount; i++)
        {
            segments[i].AddOverlay(darkOverlayPrefab, out segments[i].darkOverlay);

            if (flickerNew && i == darkCount - 1)
                StartCoroutine(FlickerThenEnable(segments[i].darkOverlay, true));
        }

        if (flickerNew && currentHealth <= 5)
        {
            int index = currentHealth - 1;
            if (index >= 0 && index < maxBase)
            {
                segments[index].SetActive(false);
                StartCoroutine(FlickerThenEnable(segments[index].baseGreen, false));
            }
        }
    }


    private IEnumerator FlickerThenDisable(GameObject obj, bool destroyAfter = false, float delay = 0.05f)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null) yield break;

        for (int i = 0; i < 2; i++)
        {
            img.enabled = false;
            yield return new WaitForSeconds(delay);
            img.enabled = true;
            yield return new WaitForSeconds(delay);
        }

        if (destroyAfter)
            Destroy(obj);
        else
            obj.SetActive(false);
    }

    private IEnumerator FlickerThenEnable(GameObject obj, bool isOverlay)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null) yield break;

        for (int i = 0; i < 3; i++)
        {
            img.enabled = false;
            yield return new WaitForSeconds(0.05f - (i * 0.01f));
            img.enabled = true;
            yield return new WaitForSeconds(0.05f - (i * 0.01f));
        }

        if (!isOverlay)
            obj.SetActive(true);
    }

    public int GetCurrentHealth() => currentHealth;
}

