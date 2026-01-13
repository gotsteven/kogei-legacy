using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLighting : MonoBehaviour
{
    [Header("設定")]
    public Light2D globalLight; // シーン全体を照らすライト

    [Header("色の移り変わり設定")]
    [Tooltip("左端が0:00(夜)、真ん中が12:00(昼)、右端が24:00(夜)")]
    public Gradient dayNightGradient; // グラデーション設定用

    private void Update()
    {
        if (TimeManager.Instance == null) return;
        if (globalLight == null) return;

        // 現在の時間を取得
        float time = TimeManager.Instance.normalizedTime;

        Color currentColor = dayNightGradient.Evaluate(time);

        // ライトの色に反映させる
        globalLight.color = currentColor;
    }
}