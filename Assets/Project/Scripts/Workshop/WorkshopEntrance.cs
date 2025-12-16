using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WorkshopEntrance : MonoBehaviour
{
    [SerializeField] private string workshopSceneName = "Workshop"; // 工房内シーン名
    [SerializeField] private GameObject promptUI; // 「Fで入る」表示

    private bool playerInRange = false; // プレイヤーが範囲内か

    void Start()
    {
        // 最初はUIを非表示
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        // プレイヤーが範囲内でFキーを押したら
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            EnterWorkshop();
        }
    }

    // プレイヤーが範囲に入った
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null)
                promptUI.SetActive(true); // UIを表示
        }
    }

    // プレイヤーが範囲から出た
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null)
                promptUI.SetActive(false); // UIを非表示
        }
    }

    void EnterWorkshop()
    {
        SceneManager.LoadScene(workshopSceneName);
    }
}
