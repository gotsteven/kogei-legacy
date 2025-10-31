using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PotteryGameManager : MonoBehaviour
{
    [Header("ゲーム設定")]
    [SerializeField] private float gameTime = 20f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float countdownTime = 3f; // カウントダウン時間

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI countdownText; // 追加
    [SerializeField] private RawImage referenceImage;
    [SerializeField] private GameObject resultPanel;

    [Header("お題表示")]
    [SerializeField] private ReferencePotteryDisplay referenceDisplay;

    [Header("お題設定")]
    [SerializeField] private float[] targetRadius;

    private ProceduralPottery pottery;
    private float remainingTime;
    private bool isGameActive = false;
    private bool isCountingDown = false; // 追加

    void Start()
    {
        pottery = GetComponent<ProceduralPottery>();
        remainingTime = gameTime;

        // お題の半径を設定
        SetupTargetShape();

        // お題の壺を表示
        if (referenceDisplay != null)
        {
            referenceDisplay.GenerateReferenceDisplay(targetRadius);
        }

        if (resultPanel != null)
            resultPanel.SetActive(false);

        // スコアテキストを非表示にする
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        // カウントダウンを開始
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        isCountingDown = true;

        // PotteryDeformerを一時的に無効化
        PotteryDeformer deformer = GetComponent<PotteryDeformer>();
        if (deformer != null)
        {
            deformer.enabled = false;
        }

        // カウントダウン表示
        float countdown = countdownTime;

        while (countdown > 0)
        {
            // 整数部分を取得（3, 2, 1）
            int displayNumber = Mathf.CeilToInt(countdown);

            if (countdownText != null)
            {
                countdownText.text = displayNumber.ToString();
            }

            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        // "スタート！"を表示
        if (countdownText != null)
        {
            countdownText.text = "スタート！";
        }

        yield return new WaitForSeconds(0.5f);

        // カウントダウンテキストを非表示
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // ゲーム開始
        isCountingDown = false;
        isGameActive = true;

        // PotteryDeformerを有効化
        if (deformer != null)
        {
            deformer.enabled = true;
        }
    }

    void Update()
    {
        // カウントダウン中は何もしない
        if (isCountingDown)
        {
            // 壺は回転させる（見栄えのため）
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            return;
        }

        if (isGameActive)
        {
            // 壺を回転
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

            // タイマー更新
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0f)
            {
                EndGame();
            }
        }
    }

    void SetupTargetShape()
    {
        int segments = pottery.GetHeightSegmentCount() + 1;
        targetRadius = new float[segments];

        // お題の壺の形を定義（例：くびれのある壺）
        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);

            // 底部（太い） → 中央（細い） → 口（広がる）
            if (t < 0.3f)
            {
                // 底部
                targetRadius[i] = Mathf.Lerp(0.6f, 0.7f, t / 0.3f);
            }
            else if (t < 0.7f)
            {
                // くびれ
                float localT = (t - 0.3f) / 0.4f;
                targetRadius[i] = 0.7f - Mathf.Sin(localT * Mathf.PI) * 0.25f;
            }
            else
            {
                // 口（広がる）
                float localT = (t - 0.7f) / 0.3f;
                targetRadius[i] = Mathf.Lerp(0.45f, 0.6f, localT);
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
        }
    }

    void EndGame()
    {
        isGameActive = false;

        // 削り操作を無効化
        PotteryDeformer deformer = GetComponent<PotteryDeformer>();
        if (deformer != null)
        {
            deformer.enabled = false;
        }

        // 回転も停止させる
        rotationSpeed = 0f;

        float score = CalculateScore();

        // スコアテキストを表示
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = $"スコア: {score:F1}点";
        }

        if (resultPanel != null)
            resultPanel.SetActive(true);

        Debug.Log($"finish score: {score}");
    }

    float CalculateScore()
    {
        float totalError = 0f;
        int segments = pottery.GetHeightSegmentCount() + 1;

        for (int i = 0; i < segments; i++)
        {
            float diff = Mathf.Abs(pottery.radius[i] - targetRadius[i]);
            totalError += diff;
        }

        float maxPossibleError = segments * 1.0f;
        float normalizedError = totalError / maxPossibleError;
        float score = Mathf.Max(0f, 100f * (1f - normalizedError));

        return score;
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    public float[] GetTargetRadius()
    {
        return targetRadius;
    }
}
