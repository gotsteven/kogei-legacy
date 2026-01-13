using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    [Header("ステータス")]
    public int currentDay = 1;
    public string lastNightLog = "";
    public bool isMorning = false;

    [Header("オーディオ")]
    public AudioSource audioSource;
    public AudioClip snoreSound;
    public AudioClip breakSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSleepSequence(GameObject sleepPanel, TextMeshProUGUI dayText)
    {
        StartCoroutine(SleepCoroutine(sleepPanel, dayText));
    }

    private IEnumerator SleepCoroutine(GameObject sleepPanel, TextMeshProUGUI dayText)
    {
        Debug.Log("盗難システムを開始します");

        currentDay++;
        Debug.Log("Day " + currentDay);

        Image panelImage = null;

        if (sleepPanel != null)
        {
            sleepPanel.SetActive(true);
            panelImage = sleepPanel.GetComponent<Image>();

            if (panelImage != null)
            {
                panelImage.color = new Color(0, 0, 0, 0);
            }
        }

        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
            dayText.gameObject.SetActive(false);
        }

        float fadeDuration = 1.0f;
        float timer = 0f;

        while (timer < fadeDuration)　// 暗転演出
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            if (panelImage != null)
            {
                panelImage.color = new Color(0, 0, 0, alpha);
            }
            yield return null;
        }

        if (panelImage != null) panelImage.color = Color.black;

        if (dayText != null)
        {
            dayText.gameObject.SetActive(true); // テキスト表示
        }

        yield return new WaitForSeconds(0.5f);

        bool isTheft = Random.value > 0.5f;

        if (isTheft)
        {
            audioSource.PlayOneShot(breakSound);
            lastNightLog = "昨晩、ガラスの割れる音がした。\n工房のアイテムが盗まれたようだ...";
        }
        else
        {
            audioSource.PlayOneShot(snoreSound);
            lastNightLog = "昨晩は静かな夜だった。\n今日も素晴らしい1日にしよう。";
        }

        yield return new WaitForSeconds(5.0f);

        isMorning = true;
        GameData.lastExitDirection = "Workshop";
        SceneManager.LoadScene("Level1_Village");
    }

    public void OnVillageLoaded(GameObject player, GameObject panel, TextMeshProUGUI logText)
    {
        if (!isMorning) return;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        if (logText != null)
        {
            logText.text = $"【 {currentDay}日目 】\n\n{lastNightLog}";
        }

        isMorning = false;
    }
}