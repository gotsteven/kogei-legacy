using UnityEngine;
using UnityEngine.SceneManagement;

public class KogeiCreate: MonoBehaviour
{
    [Header("次に移動するシーン（2番目のゲーム）")]
    [SerializeField] private string nextSceneName = "PotteryGame";

    // ワークショップの「作成ボタン」にこれを割り当てる
    public void OnClickCreateAritayaki()
    {
        // 1. コードで直接ファイルを指定してロード（これが一番確実）
        KogeiData data = Resources.Load<KogeiData>("Aritayaki"); // ファイル名注意

        if (data == null)
        {
            Debug.LogError("【エラー】Resourcesフォルダにデータが見つかりません！");
            return;
        }

        // 2. ★ここが一番重要★
        // MinigameControllerが待っている「予約席」にデータを置く
        MinigameController.NextCraftDataRequest = data;
        
        // 3. 次のシーンへ
        SceneManager.LoadScene(nextSceneName);
    }
}