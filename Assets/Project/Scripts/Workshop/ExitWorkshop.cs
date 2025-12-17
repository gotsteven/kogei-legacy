using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitWorkshop : MonoBehaviour
{
    [SerializeField] private string outsideSceneName = "OutsideScene"; // 外のシーン名
    [SerializeField] private string exitDirection = "Right"; // どの方向から出たか（"Left", "Right"など）

    // ボタンのOnClick()に設定
    public void ExitToOutside()
    {
        // シーン遷移前に出口方向を設定
        GameData.lastExitDirection = exitDirection;

        // シーン遷移
        SceneManager.LoadScene(outsideSceneName);
    }
}
