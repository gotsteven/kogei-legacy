using UnityEngine;
using System.Collections; // ★★★ この行を追加 ★★★

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint_Left;
    [SerializeField] private Transform spawnPoint_Right;
    [SerializeField] private Transform spawnPoint_Top;
    [SerializeField] private Transform spawnPoint_Bottom;

    // Awake() ではプレイヤーの移動のみを行います
    void Awake()
    {
        MovePlayer();
        Debug.Log("PlayerSpawner: Awake() がMovePlayerを実行");
    }

    // Start() ではコルーチンを開始するだけにします
    void Start()
    {
        // ★★★ このメソッドを開始するように変更 ★★★
        StartCoroutine(ResetSceneTransitionFlag());
    }

    // ★★★ このメソッドを新しく追加 ★★★
    IEnumerator ResetSceneTransitionFlag()
    {
        // 全てのUpdate()とLateUpdate()が終わった
        // このフレームの最後のタイミングまで待つ
        yield return new WaitForEndOfFrame(); 

        // 次のフレームが始まる前にフラグをリセット
        GameData.isSceneTransitioning = false;
        Debug.Log("PlayerSpawner: 1フレーム描画後にフラグをリセット");
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

        if (spawnPos == null)
        {
             // デフォルトのスポーン位置
             spawnPos = spawnPoint_Right; 
        }
        
        if (spawnPos != null)
        {
            // プレイヤーの位置を変更
            player.transform.position = spawnPos.position;
            
        }
    }
}