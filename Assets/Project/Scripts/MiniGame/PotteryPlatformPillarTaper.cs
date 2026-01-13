using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PotteryPlatformPillarTaper : MonoBehaviour
{

    [Tooltip("上の面の広さ倍率 (1.0で変化なし、小さくすると細くなる)")]
    [Range(0.0f, 2.0f)]
    public float topScale = 0.7f;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = Instantiate(meshFilter.sharedMesh);
        mesh.name = "TaperedCylinder";

        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y > 0)
            {
                vertices[i].x *= topScale;
                vertices[i].z *= topScale;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }
}