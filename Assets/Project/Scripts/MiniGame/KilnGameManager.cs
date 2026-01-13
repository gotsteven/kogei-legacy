using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class KilnGameManager : MonoBehaviour
{
    [Header("References")]
    public List<Kiln> kilns = new List<Kiln>();
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI averagePowerText;
    public TextMeshProUGUI countdownText;

    [Header("Instruction Panel")]
    public GameObject instructionPanel;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    public GameObject retryButtonObject;
    public GameObject nextButtonObject;

    [Header("System References")]
    public MinigameController minigameController; // コントローラーへの参照

    [Header("Settings")]
    public float countdownDuration = 3f;
    public float gameDuration = 15f;
    public float targetPower = 50f;
    public string nextSceneName = "Workshop";

    private float remainingTime;
    private bool isGameActive = false;
    public bool IsGameActive => isGameActive;

    void Start()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        if (instructionPanel != null) instructionPanel.SetActive(true);
        isGameActive = false;
    }

    public void OnStartButtonClicked()
    {
        if (instructionPanel != null) instructionPanel.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        isGameActive = false;
        if (countdownText != null) countdownText.gameObject.SetActive(true);

        for (int i = (int)countdownDuration; i > 0; i--)
        {
            if (countdownText != null) countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null) countdownText.text = "Start!!";
        yield return new WaitForSeconds(0.5f);

        if (countdownText != null) countdownText.gameObject.SetActive(false);
        StartGame();
    }

    void StartGame()
    {
        remainingTime = gameDuration;
        isGameActive = true;
        foreach (var kiln in kilns) kiln.Initialize();
    }

    void Update()
    {
        if (!isGameActive) return;

        remainingTime -= Time.deltaTime;
        if (timerText != null) timerText.text = $"残り時間: {Mathf.CeilToInt(remainingTime)}秒";

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

        float averagePower = kilns.Count > 0 ? totalPower / kilns.Count : 0f;
        if (averagePowerText != null) averagePowerText.text = $"総火力: {averagePower:F1}%";

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

        if (resultPanel != null) resultPanel.SetActive(true);

        if (resultText != null)
        {
            resultText.text = success ? "お見事!" : "残念…";
            resultText.color = success ? new Color(1f, 0.8f, 0f) : new Color(0.8f, 0.2f, 0.2f);
        }

        if (scoreText != null) scoreText.text = $"最終平均火力: {finalAverage:F1}%";

        if (retryButtonObject != null) retryButtonObject.SetActive(!success);
        if (nextButtonObject != null) nextButtonObject.SetActive(success);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextScene()
    {
        if (minigameController != null)
        {
            minigameController.OnMinigameComplete();
        }
        else
        {
            Debug.LogWarning("MinigameController is not assigned!");
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene("Workshop");
        }
    }
}