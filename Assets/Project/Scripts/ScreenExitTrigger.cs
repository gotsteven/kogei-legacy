using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExitDirection
{
    Left,
    Right,
    Top,
    Bottom
}

public class ScreenExitTrigger : MonoBehaviour
{
    [SerializeField] private string targetSceneName; // 遷移先シーン名
    [SerializeField] private ExitDirection exitDirection;  // "Left", "Right", "Top", "Bottom"

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // どの方向から出たかを記録
            GameData.lastExitDirection = exitDirection.ToString();

            // シーン遷移
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
