using UnityEngine;

public class PotteryDeformer : MonoBehaviour
{
    [Header("削り設定")]
    [SerializeField] private float deformSpeed = 1.5f; // 削る速度（増加）
    [SerializeField] private float minRadius = 0.1f; // 最小半径
    [SerializeField] private int deformRange = 3; // 削る範囲（拡大）
    [SerializeField] private float maxRayDistance = 100f; // Rayの最大距離

    private ProceduralPottery pottery;
    private Camera mainCamera;

    void Start()
    {
        pottery = GetComponent<ProceduralPottery>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DeformAtMousePosition();
        }
    }

    void DeformAtMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // RaycastAllを使って複数のヒット点を取得（より確実）
        RaycastHit[] hits = Physics.RaycastAll(ray, maxRayDistance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
            {
                // ヒット位置のY座標から高さセグメントを計算
                float localY = hit.point.y;
                int heightIndex = Mathf.RoundToInt(localY / pottery.GetHeight() * pottery.GetHeightSegmentCount());

                // 範囲制限
                heightIndex = Mathf.Clamp(heightIndex, 0, pottery.GetHeightSegmentCount());

                // 削る処理（周辺も影響）
                for (int i = -deformRange; i <= deformRange; i++)
                {
                    int targetIndex = heightIndex + i;
                    if (targetIndex >= 0 && targetIndex <= pottery.GetHeightSegmentCount())
                    {
                        // 中心からの距離に応じて削る量を減衰
                        float falloff = 1f - (Mathf.Abs(i) / (float)(deformRange + 1));
                        float deformAmount = deformSpeed * falloff * Time.deltaTime;

                        pottery.radius[targetIndex] -= deformAmount;
                        pottery.radius[targetIndex] = Mathf.Max(pottery.radius[targetIndex], minRadius);
                    }
                }

                // メッシュを更新
                pottery.UpdateMesh();

                // 最初のヒットで処理を終了
                break;
            }
        }
    }
}
