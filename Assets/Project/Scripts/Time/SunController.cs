using UnityEngine;

public class SunController : MonoBehaviour
{
    [Header("設定")]
    public Transform sunTransform;
    public float orbitRadius = 8.0f;

    public Vector2 centerOffset = new Vector2(0, -3.0f);

    private void Update()
    {
        if (TimeManager.Instance == null) return;
        if (sunTransform == null) return;

        // 現在の時間を取得
        float time = TimeManager.Instance.normalizedTime;

        float angle = 270f - (time * 360f);

        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * orbitRadius;
        float y = Mathf.Sin(rad) * orbitRadius;

        Vector3 cameraPos = Camera.main.transform.position;

        sunTransform.position = new Vector3(
            cameraPos.x + x + centerOffset.x,
            cameraPos.y + y + centerOffset.y,
            sunTransform.position.z
        );
    }
}