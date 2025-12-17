using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameController : MonoBehaviour
{
    // ミニゲーム終了時に呼ばれる関数
    public void OnMinigameComplete()
    {
        // フラグを立てる
        GameData.isCraftCompleted = true;
        GameData.completedCraftName = "有田焼"; // 工芸品名を設定
    }
}
