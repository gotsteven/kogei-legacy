using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WorkshopBedInteraction : MonoBehaviour
{
    [Header("操作するUIパネル")]
    public GameObject AnnouncePanelUI;
    public GameObject FadePanel;
    public TextMeshProUGUI dayText;

    [Header("オーディオ")]
    public AudioSource audioSource;
    public AudioClip paperSound;

    // ベッドをクリック時の処理
    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (AnnouncePanelUI != null)
        {
            AnnouncePanelUI.SetActive(true);

            if (audioSource != null && paperSound != null)
            {
                audioSource.PlayOneShot(paperSound);
            }
        }
        else
        {
            Debug.LogWarning("Announce Panel UIが設定されていません。", this);
        }
    }

    public void GoToSleep()
    {
        if (AnnouncePanelUI != null)
        {
            AnnouncePanelUI.SetActive(false);
        }

        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.StartSleepSequence(FadePanel, dayText);
        }
        else
        {
            Debug.LogError("GlobalGameManagerが見つかりません！シーンに配置されていますか？");
        }
    }

    public void CloseAnnouncePanel()
    {
        if (AnnouncePanelUI != null)
        {
            AnnouncePanelUI.SetActive(false);
        }
    }
}