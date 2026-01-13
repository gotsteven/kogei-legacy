using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("時間設定")]
    [Tooltip("1日の長さ（秒）")]
    public float dayDuration = 900f;

    [Header("停止設定")]
    [Tooltip("何時になったら時間を止めるか（24時間表記）")]
    public float stopHour = 20f;

    [Header("状態")]
    public float currentTime = 0f;
    public bool isTimeStopped = false;

    // 0.0(0:00) 〜 1.0(24:00)
    public float normalizedTime
    {
        get
        {
            if (dayDuration <= 0) return 0f;
            return currentTime / dayDuration;
        }
    }

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

    private void Update()
    {
        if (isTimeStopped) return;

        currentTime += Time.deltaTime;

        // 指定時間（stopHour）で停止
        float stopTimePoint = dayDuration * (stopHour / 24f);

        if (currentTime >= stopTimePoint)
        {
            currentTime = stopTimePoint;
            isTimeStopped = true;
            Debug.Log($"【TimeManager】現在 {stopHour}時 です。時間が止まりました。");
        }
    }

    public void Sleep()
    {
        currentTime = 0f;
        isTimeStopped = false;
        Debug.Log("【TimeManager】起床！新しい一日が始まりました。");
    }
}