using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [Header("Settings")]
    public string craftName = "有田焼";

    // ミニゲーム終了時に呼ばれる関数
    public void OnMinigameComplete()
    {
        // フラグを立てる
        GameData.isCraftCompleted = true;
        GameData.completedCraftName = craftName;

        Debug.Log($"Craft Completed: {craftName}");
    }
}