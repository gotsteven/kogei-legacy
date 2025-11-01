using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    // [インスペクターで設定] 表示・非表示を切り替えるUIパネル
    public GameObject AnnouncePanelUI;

    // ベッドをクリックした時の処理
    private void OnMouseDown() // 2Dオブジェクト用
    {
        // UIパネルが設定されているか確認
        if (AnnouncePanelUI != null)
        {
            // UIパネルの表示状態を反転させる (表示 -> 非表示, 非表示 -> 表示)
            AnnouncePanelUI.SetActive(!AnnouncePanelUI.activeSelf);
            Debug.Log("ベッドがクリックされました！UI表示状態: " + AnnouncePanelUI.activeSelf);
        }
        else
        {
            Debug.LogWarning("Announce Panel UIが設定されていません！インスペクターで設定してください。", this);
        }
    }

    public void CloseAnnouncePanel()
    {
        if (AnnouncePanelUI != null)
        {
            AnnouncePanelUI.SetActive(false); // 常に非表示にする
        }
    }
}