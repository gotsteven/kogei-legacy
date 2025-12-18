using UnityEngine;

public class AreaWarpTrigger : MonoBehaviour
{
    [Header("ワープ先の設定")]
    public Vector3 playerTargetPos; // プレイヤーの移動先座標
    public Vector3 cameraTargetPos; // カメラの移動先座標（通常 Z は -10）

    private void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーがトリガーに触れたか判定
        if (other.CompareTag("Player"))
        {
            // 1. プレイヤーをワープ
            other.transform.position = playerTargetPos;

            // 2. メインカメラをワープ
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                // Z座標は現在の値を維持（0になると何も映らなくなるため）
                Vector3 newCamPos = cameraTargetPos;
                newCamPos.z = mainCam.transform.position.z;
                mainCam.transform.position = newCamPos;
            }

            Debug.Log("Area Warp Executed");
        }
    }
}