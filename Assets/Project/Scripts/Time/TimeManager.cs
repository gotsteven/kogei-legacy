using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("時間設定")]
    [Tooltip("ゲーム内の1日(秒計算)")]
    public float dayDurationInSeconds = 900f;

    [Header("開始時間")]
    [Tooltip("ゲーム開始時の時間（0.0=深夜0時, 0.25=朝6時, 0.5=正午）")]
    [Range(0f, 1f)]
    public float startTime = 0.25f; // デフォルトは朝6時から

    [Header("現在の状態（確認用）")]
    [Range(0f, 1f)]
    public float normalizedTime = 0f; // 0.0 〜 1.0 で時間を表す

    private void Awake()
    {
        // シングルトン化（どこからでもアクセスできるようにする）
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        normalizedTime = startTime;
    }

    private void Update()
    {
        normalizedTime += Time.deltaTime / dayDurationInSeconds;

        // 1.0（深夜24時）を超えたら 0.0（深夜0時）に戻す
        if (normalizedTime >= 1f)
        {
            normalizedTime = 0f;
        }
    }

    // 外部から時間を聞くための関数（0〜24時で返す）
    public float GetCurrentHour()
    {
        return normalizedTime * 24f;
    }
}