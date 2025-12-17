using UnityEngine;
using TMPro;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private GameObject completionPanel; // 完成通知パネル
    [SerializeField] private TextMeshProUGUI completionText; // 完成メッセージ

    void Start()
    {
        // ミニゲームから戻ってきたかチェック
        if (GameData.isCraftCompleted)
        {
            ShowCompletionNotice();

            // フラグをリセット（重要！）
            GameData.isCraftCompleted = false;
        }
    }

    void ShowCompletionNotice()
    {
        completionPanel.SetActive(true);
        completionText.text = $"{GameData.completedCraftName}が完成しました！";
    }

    // ボタンで通知を閉じる
    public void CloseNotice()
    {
        completionPanel.SetActive(false);
    }
}
