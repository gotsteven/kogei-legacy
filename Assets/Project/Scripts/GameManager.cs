using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public List<Kiln> kilns = new List<Kiln>();
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI averagePowerText;
    public TextMeshProUGUI countdownText; // カウントダウン用テキスト

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    [Header("Settings")]
    public float countdownDuration = 3f; // カウントダウン秒数
    public float gameDuration = 20f;
    public float targetPower = 50f;

    private float remainingTime;
    private bool isGameActive = false;
    public bool IsGameActive => isGameActive; // 他のスクリプトから参照できるようにする

    void Start()
    {
        // 結果パネルを非表示に設定（念のため）
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        // カウントダウンから開始
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        isGameActive = false; // ゲームはまだ開始していない

        // カウントダウンテキストを表示
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        // 3, 2, 1 とカウントダウン
        for (int i = (int)countdownDuration; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSeconds(1f);
        }

        // "スタート!" 表示
        if (countdownText != null)
        {
            countdownText.text = "スタート!";
        }
        yield return new WaitForSeconds(0.5f);

        // カウントダウンテキストを非表示
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // ゲーム本編開始
        StartGame();
    }

    void StartGame()
    {
        remainingTime = gameDuration;
        isGameActive = true;

        // 窯の初期化
        foreach (var kiln in kilns)
        {
            kiln.Initialize();
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        remainingTime -= Time.deltaTime;
        timerText.text = $"残り時間: {Mathf.CeilToInt(remainingTime)}秒";

        float totalPower = 0f;
        bool anyBroken = false;

        foreach (var kiln in kilns)
        {
            totalPower += kiln.CurrentPower;
            if (kiln.IsBroken)
            {
                anyBroken = true;
                break;
            }
        }

        float averagePower = totalPower / kilns.Count;
        averagePowerText.text = $"総火力: {averagePower:F1}%";

        if (anyBroken)
        {
            EndGame(false, averagePower);
        }
        else if (remainingTime <= 0)
        {
            EndGame(true, averagePower);
        }
    }

    void EndGame(bool success, float finalAverage)
    {
        isGameActive = false;

        // 結果パネルを表示
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        // 結果テキストを更新
        if (resultText != null)
        {
            resultText.text = success ? "お見事!" : "残念…";
            resultText.color = success ? new Color(1f, 0.8f, 0f) : new Color(0.8f, 0.2f, 0.2f);
        }

        // スコアテキストを更新
        if (scoreText != null)
        {
            scoreText.text = $"最終平均火力: {finalAverage:F1}%";
        }

        Debug.Log($"{(success ? "お見事!" : "残念…")} 最終平均火力: {finalAverage:F1}%");
    }

    // リトライボタン用
    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
