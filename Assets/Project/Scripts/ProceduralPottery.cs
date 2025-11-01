using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ProceduralPottery : MonoBehaviour
{
    [Header("メッシュ設定")]
    [SerializeField] private int heightSegments = 25; // 高さ方向の分割数
    [SerializeField] private int radialSegments = 32; // 円周方向の分割数
    [SerializeField] private float height = 2f; // 壺の高さ
    [SerializeField] private float initialRadius = 0.8f; // 初期半径

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public float[] radius; // 各高さの半径（外部からアクセス可能）

    void Start()
    {
        GeneratePottery();
    }

    public void GeneratePottery()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // 半径配列を初期化
        radius = new float[heightSegments + 1];
        for (int i = 0; i <= heightSegments; i++)
        {
            radius[i] = initialRadius;
        }

        CreateMeshData();
        UpdateMesh();
    }

    void CreateMeshData()
    {
        // 頂点数：(heightSegments + 1) * (radialSegments + 1)
        vertices = new Vector3[(heightSegments + 1) * (radialSegments + 1)];

        // 頂点を配置
        for (int h = 0; h <= heightSegments; h++)
        {
            float y = (float)h / heightSegments * height;

            for (int r = 0; r <= radialSegments; r++)
            {
                float angle = (float)r / radialSegments * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * radius[h];
                float z = Mathf.Sin(angle) * radius[h];

                int index = h * (radialSegments + 1) + r;
                vertices[index] = new Vector3(x, y, z);
            }
        }

        // 三角形インデックスを作成
        triangles = new int[heightSegments * radialSegments * 6];
        int triIndex = 0;

        for (int h = 0; h < heightSegments; h++)
        {
            for (int r = 0; r < radialSegments; r++)
            {
                int current = h * (radialSegments + 1) + r;
                int next = current + radialSegments + 1;

                // 四角形を2つの三角形に分割
                triangles[triIndex++] = current;
                triangles[triIndex++] = next;
                triangles[triIndex++] = current + 1;

                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;
                triangles[triIndex++] = next + 1;
            }
        }
    }

    public void UpdateMesh()
    {
        // 半径の変更を反映して頂点位置を更新
        for (int h = 0; h <= heightSegments; h++)
        {
            for (int r = 0; r <= radialSegments; r++)
            {
                float angle = (float)r / radialSegments * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * radius[h];
                float z = Mathf.Sin(angle) * radius[h];

                int index = h * (radialSegments + 1) + r;
                vertices[index] = new Vector3(x, vertices[index].y, z);
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // コライダーを更新（お題壺には不要だが念のため）
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider != null)
        {
            collider.sharedMesh = mesh;
        }

        // デバッグ用：メッシュ更新を確認
        Debug.Log($"UpdateMesh called. Vertex count: {vertices.Length}");
    }



    public int GetHeightSegmentCount()
    {
        return heightSegments;
    }

    public float GetHeight()
    {
        return height;
    }
}
