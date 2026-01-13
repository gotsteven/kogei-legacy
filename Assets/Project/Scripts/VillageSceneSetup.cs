using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VillageSceneSetup : MonoBehaviour
{
    [Header("操作する対象")]
    public GameObject player; 
    public GameObject dayLogPanel;
    public TextMeshProUGUI dayTitleText;
    public TextMeshProUGUI dayLogText;

    [Header("演出用")]
    public Image fadePanel;

    [Header("オーディオ")] 
    public AudioSource audioSource;
    public AudioClip paperSound;

    void Start()
    {
        if (GlobalGameManager.Instance != null && GlobalGameManager.Instance.isMorning)
        {
            StartCoroutine(WakeUpSequence());
        }
        else
        {
            if (fadePanel != null)
            {
                fadePanel.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator WakeUpSequence()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = Color.black;
        }

        float fadeDuration = 1.0f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeDuration);

            if (fadePanel != null)
            {
                fadePanel.color = new Color(0, 0, 0, alpha);
            }
            yield return null;
        }

        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        if (audioSource != null && paperSound != null)
        {
            audioSource.PlayOneShot(paperSound);
        }

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.OnVillageLoaded(player, dayLogPanel, dayTitleText, dayLogText);
        }
    }
}