using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PotteryGameManager : MonoBehaviour
{
    [Header("ゲーム設定")]
    [SerializeField] private float gameTime = 20f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float countdownTime = 3f;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private RawImage referenceImage;
    [SerializeField] private GameObject resultPanel;
    
    [Header("説明パネル")] // 追加
    [SerializeField] private GameObject instructionPanel;
    
    [Header("お題表示")]
    [SerializeField] private ReferencePotteryDisplay referenceDisplay;
    
    [Header("お題設定")]
    [SerializeField] private float[] targetRadius;
    
    private ProceduralPottery pottery;
    private float remainingTime;
    private bool isGameActive = false;
    private bool isCountingDown = false;

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
        
        // 結果パネルを非表示
        if (resultPanel != null)
            resultPanel.SetActive(false);
        
        // スコアテキストを非表示
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);
        
        // カウントダウンテキストを非表示
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
        
        // 説明パネルを表示
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }
        
        // ゲームはまだ開始していない
        isGameActive = false;
    }

    // StartButtonから呼ばれる
    public void OnStartButtonClicked()
    {
        // 説明パネルを非表示
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }
        
        // カウントダウン開始
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
        
        // カウントダウンテキストを表示
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }
        
        // 3, 2, 1 とカウントダウン
        for (int i = (int)countdownTime; i > 0; i--)
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
        // カウントダウン中は回転のみ
        if (isCountingDown)
        {
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
        
        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            
            if (t < 0.3f)
            {
                targetRadius[i] = Mathf.Lerp(0.6f, 0.7f, t / 0.3f);
            }
            else if (t < 0.7f)
            {
                float localT = (t - 0.3f) / 0.4f;
                targetRadius[i] = 0.7f - Mathf.Sin(localT * Mathf.PI) * 0.25f;
            }
            else
            {
                float localT = (t - 0.7f) / 0.3f;
                targetRadius[i] = Mathf.Lerp(0.45f, 0.6f, localT);
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"残り時間: {Mathf.CeilToInt(remainingTime)}秒";
        }
    }

    void EndGame()
    {
        isGameActive = false;
        
        // システムカーソルを表示（非表示にしている場合）
        Cursor.visible = true;
        
        // 削り操作を無効化
        PotteryDeformer deformer = GetComponent<PotteryDeformer>();
        if (deformer != null)
        {
            deformer.enabled = false;
        }
        
        // 回転も停止
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
        
        Debug.Log($"ゲーム終了！スコア: {score}");
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
    
    // リトライボタン用
    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
