using UnityEngine;

public class SunController : MonoBehaviour
{
    [Header("設定")]
    public Transform sunTransform;
    public float orbitRadius = 8.0f;
    public Vector2 centerOffset = new Vector2(0, -3.0f);

    private void Start()
    {
        if (sunTransform == null) sunTransform = transform;
    }

    private void LateUpdate()
    {
        // 必要なコンポーネントがなければ処理しない（安全装置）
        if (TimeManager.Instance == null) return;
        if (sunTransform == null) return;

        // 毎回現在のメインカメラを取得（シーン遷移対策）
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        // 計算
        float time = TimeManager.Instance.normalizedTime;
        float angle = 270f - (time * 360f);
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * orbitRadius;
        float y = Mathf.Sin(rad) * orbitRadius;

        Vector3 cameraPos = mainCam.transform.position;

        // 位置更新
        sunTransform.position = new Vector3(
            cameraPos.x + x + centerOffset.x,
            cameraPos.y + y + centerOffset.y,
            sunTransform.position.z
        );
    }
}