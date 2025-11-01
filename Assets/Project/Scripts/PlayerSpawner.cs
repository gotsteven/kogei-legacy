using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // playerPrefabは不要になるので削除
    [SerializeField] private Transform spawnPoint_Left;
    [SerializeField] private Transform spawnPoint_Right;
    [SerializeField] private Transform spawnPoint_Top;
    [SerializeField] private Transform spawnPoint_Bottom;

    void Start()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // 既存のプレイヤーを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("プレイヤーが見つかりません");
            return;
        }

        Transform spawnPos = null;

        // 来た方向に応じてスポーン位置を決める
        switch (GameData.lastExitDirection)
        {
            case "Left":
                spawnPos = spawnPoint_Right;
                break;
            case "Right":
                spawnPos = spawnPoint_Left;
                break;
            case "Top":
                spawnPos = spawnPoint_Bottom;
                break;
            case "Bottom":
                spawnPos = spawnPoint_Top;
                break;
            default:
                spawnPos = spawnPoint_Left;
                break;
        }

        if (spawnPos != null)
        {
            // プレイヤーの位置を変更
            player.transform.position = spawnPos.position;
        }
    }
}
