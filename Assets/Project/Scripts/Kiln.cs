using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Kiln : MonoBehaviour, IPointerUpHandler
{
    [Header("UI References")]
    public Slider powerBar;
    public Image opening; // 窯の開口部
    public ParticleSystem fireParticles;

    [Header("Settings")]
    public float initialPower = 70f;
    public float minDecayRate = 0.05f;
    public float maxDecayRate = 0.1f;
    public float firewoodBoost = 35f;
    public Color lowFireColor = new Color(0.3f, 0.1f, 0.1f);
    public Color highFireColor = new Color(1f, 0.5f, 0f);

    private float currentPower;
    private float decayRate;
    private bool isBroken = false;

    public float CurrentPower => currentPower;
    public bool IsBroken => isBroken;

    public void Initialize()
    {
        currentPower = initialPower;
        decayRate = Random.Range(minDecayRate, maxDecayRate);
        isBroken = false;

        if (powerBar != null)
        {
            powerBar.maxValue = 100f;
            powerBar.value = currentPower;
        }

        // パーティクル設定
        if (fireParticles != null)
        {
            var main = fireParticles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(1f, 0.5f, 0f),
                new Color(1f, 1f, 0f)
            );
            main.startLifetime = new ParticleSystem.MinMaxCurve(1f, 1.5f);
        }
    }

    void Update()
    {
        if (isBroken) return;

        // 火力減衰
        currentPower -= decayRate;
        currentPower = Mathf.Max(0f, currentPower);

        // UI更新
        if (powerBar != null)
        {
            powerBar.value = currentPower;
        }

        // 開口部の色更新
        if (opening != null)
        {
            opening.color = Color.Lerp(lowFireColor, highFireColor, currentPower / 100f);
        }

        // パーティクル更新
        if (fireParticles != null)
        {
            var emission = fireParticles.emission;
            emission.rateOverTime = currentPower * 0.5f; // 火力に応じた放出量
        }

        // 破損チェック
        if (currentPower <= 0f)
        {
            isBroken = true;
            if (fireParticles != null)
            {
                fireParticles.Stop();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // FirewoodManagerから呼ばれる
        if (FirewoodManager.Instance != null && FirewoodManager.Instance.IsDragging)
        {
            AddFirewood();
        }
    }

    public void AddFirewood()
    {
        if (isBroken) return;

        currentPower = Mathf.Min(100f, currentPower + firewoodBoost);

        // 薪投入時のパーティクル強化
        if (fireParticles != null)
        {
            fireParticles.Emit(30);
        }
    }
}
