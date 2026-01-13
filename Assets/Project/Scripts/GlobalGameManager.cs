using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance; // どこからでも呼べる

    [Header("ステータス")]
    public int currentDay = 1; // 現在の日にち
    public string lastNightLog = ""; // 昨晩のログ
    public bool isMorning = false; // 朝かどうかのフラグ

    [Header("オーディオ")]
    public AudioSource audioSource;
    public AudioClip snoreSound; // いびき
    public AudioClip breakSound; // ガラスが割れる音

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

    //「はい」ボタンから呼ばれる関数
    public void StartSleepSequence()
    {
        StartCoroutine(SleepCoroutine());
    }

    private IEnumerator SleepCoroutine()
    {
        Debug.Log("画面暗転...");

        currentDay++;
        Debug.Log("Day " + currentDay);

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

        yield return new WaitForSeconds(4.0f);

        isMorning = true;
        SceneManager.LoadScene("Level1_Village");
    }

    // シーン読み込み完了時
    public void OnVillageLoaded(GameObject player, GameObject panel, TextMeshProUGUI logText)
    {
        if (!isMorning) return;

        GameObject spawnPoint = GameObject.Find("WorkshopSpawnPoint");
        if (spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }

        panel.SetActive(true);
        logText.text = $"【 {currentDay}日目 】\n\n{lastNightLog}";

        isMorning = false;
    }
}