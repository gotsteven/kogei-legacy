using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // シーン名を指定して遷移
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
