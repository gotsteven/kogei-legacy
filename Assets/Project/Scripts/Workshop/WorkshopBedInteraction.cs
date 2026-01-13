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

    [Header("ヒント表示")]
    public GameObject interactionHintUI;

    [Header("オーディオ")]
    public AudioSource audioSource;
    public AudioClip paperSound;

    private void Start()
    {
        if (interactionHintUI != null) interactionHintUI.SetActive(true);
        if (AnnouncePanelUI != null) AnnouncePanelUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (AnnouncePanelUI != null && !AnnouncePanelUI.activeSelf)
            {
                OpenSleepPanel();
            }
        }
    }

    // ベッドをクリック時の処理
    private void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (AnnouncePanelUI != null && !AnnouncePanelUI.activeSelf)
        {
            OpenSleepPanel();
        }
    }

    private void OpenSleepPanel()
    {
        if (interactionHintUI != null)
        {
            interactionHintUI.SetActive(false);
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

        if (interactionHintUI != null)
        {
            interactionHintUI.SetActive(true);
        }
    }
}