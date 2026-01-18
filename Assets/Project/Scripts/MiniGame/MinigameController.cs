using UnityEngine;
using UnityEngine.SceneManagement;
using System; 

public class MinigameController : MonoBehaviour
{
    // ★データ受け取り用の窓口（ここに入っているはず！）
    public static KogeiData NextCraftDataRequest;

    // 内部で保持する変数（インスペクター設定はもう不要なので private で隠す）
    private KogeiData targetKogeiData;

    void Start()
    {
        // 1. 共有ボックスからデータを取り出す
        targetKogeiData = NextCraftDataRequest;

        // 2. もし空っぽなら、それはバグかテストプレイ方法の間違い
        if (targetKogeiData == null)
        {
            Debug.LogError("【致命的エラー】データが届いていません！\n" +
                           "※このシーンを直接再生していませんか？必ずWorkShopシーンから始めてください。");
            return;
        }

        Debug.Log($"【受信成功】{targetKogeiData.workName} の作成を開始します。");
    }

    public void OnMinigameComplete()
    {
        // データがない場合は処理しない（Startでエラー出てるはずなので）
        if (targetKogeiData == null) return;

        // --- 保存処理 ---
        GameObject newCraft = new GameObject(targetKogeiData.workName);
        
        KogeiItem itemScript = newCraft.AddComponent<KogeiItem>();
        itemScript.data = targetKogeiData;
        itemScript.uniqueID = Guid.NewGuid().ToString();
        itemScript.createdAt = DateTime.Now.ToString();

        GameData.isCraftCompleted = true;
        GameData.completedCraftName = targetKogeiData.workName;

        Debug.Log($"Craft Completed: {targetKogeiData.workName}");
        // 倉庫へ登録
        CraftStorage.Register(newCraft);
        CraftStorage.TempResultData = targetKogeiData;

        // 工房へ戻る
        SceneManager.LoadScene("WorkShop");
    }
}

