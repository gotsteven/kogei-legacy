using UnityEngine;
using System.Collections;

public class ReferencePotteryDisplay : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Camera referenceCamera;
    [SerializeField] private Vector3 displayPosition = new Vector3(10, 1, 0);
    [SerializeField] private float cameraDistance = 3f;

    [Header("マテリアル")]
    [SerializeField] private Material potteryMaterial;

    private GameObject referencePotteryObject;
    private ProceduralPottery referencePottery;
    private int referencePotteryLayer;

    void Awake()
    {
        referencePotteryLayer = LayerMask.NameToLayer("ReferencePottery");
        referenceCamera.enabled = false;
        if (referencePotteryLayer == -1)
        {
            Debug.LogError("ReferencePottery layer not found! Please create it.");
        }
    }

    public void GenerateReferenceDisplay(float[] targetRadius)
    {
        if (targetRadius == null || targetRadius.Length == 0)
        {
            Debug.LogError("targetRadius is null or empty!");
            return;
        }

        StartCoroutine(GenerateReferenceDisplayCoroutine(targetRadius));
    }

    private IEnumerator GenerateReferenceDisplayCoroutine(float[] targetRadius)
    {
        if (referencePotteryObject != null)
        {
            Destroy(referencePotteryObject);
        }

        // お題壺を生成
        referencePotteryObject = new GameObject("ReferencePotteryDisplay");
        referencePotteryObject.transform.position = displayPosition;
        referencePotteryObject.layer = referencePotteryLayer;

        // ProceduralPotteryコンポーネントを追加
        referencePottery = referencePotteryObject.AddComponent<ProceduralPottery>();

        // 1フレーム待ってコンポーネントが完全に初期化されるのを待つ
        yield return null;

        // まずGeneratePottery()を呼んで基本構造を作る
        referencePottery.GeneratePottery();

        Debug.Log($"Initial pottery generated. Radius[0]={referencePottery.radius[0]}, Radius[10]={referencePottery.radius[10]}");

        // もう1フレーム待つ
        yield return null;

        // targetRadiusを直接radiusに代入
        if (referencePottery.radius != null && targetRadius.Length > 0)
        {
            int minLength = Mathf.Min(targetRadius.Length, referencePottery.radius.Length);

            // 配列をコピー
            for (int i = 0; i < minLength; i++)
            {
                referencePottery.radius[i] = targetRadius[i];
            }

            Debug.Log($"Target radius set. Radius[0]={referencePottery.radius[0]}, Radius[10]={referencePottery.radius[10]}");
            Debug.Log($"Expected values: targetRadius[0]={targetRadius[0]}, targetRadius[10]={targetRadius[10]}");

            // もう1フレーム待つ
            yield return null;

            // UpdateMesh()を呼んで形状を反映
            referencePottery.UpdateMesh();

            Debug.Log($"After UpdateMesh. Radius[0]={referencePottery.radius[0]}, Radius[10]={referencePottery.radius[10]}");
        }
        else
        {
            Debug.LogError($"Radius array error! referencePottery.radius null? {referencePottery.radius == null}, length: {referencePottery.radius?.Length}");
        }

        // マテリアルを設定
        MeshRenderer meshRenderer = referencePotteryObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && potteryMaterial != null)
        {
            meshRenderer.material = potteryMaterial;
            Debug.Log("Material applied");
        }
        if (referenceCamera != null)
        {
            referenceCamera.enabled = true;
            Debug.Log("Reference camera enabled - display completed");
        }
    }

    void OnDestroy()
    {
        if (referencePotteryObject != null)
        {
            Destroy(referencePotteryObject);
        }
    }
}
