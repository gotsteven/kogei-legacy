using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkShopController : MonoBehaviour
{
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private TextMeshProUGUI completionText;
    [SerializeField] private Image completionImage;

    void Start()
    {
        if (CraftStorage.TempResultData != null)
        {
            ShowDetailedNotice(CraftStorage.TempResultData);
        }
        else if (GameData.isCraftCompleted)
        {
            ShowSimpleNotice();
            GameData.isCraftCompleted = false;
        }
    }

    void ShowDetailedNotice(KogeiData data)
    {
        completionPanel.SetActive(true);

        if (completionText != null)
        {
            completionText.text = $"{data.workName}が完成しました！";
        }

        if (completionImage != null && data.artworkImage != null)
        {
            completionImage.sprite = data.artworkImage;
            completionImage.preserveAspect = true;
            completionImage.gameObject.SetActive(true);
        }
    }

    void ShowSimpleNotice()
    {
        completionPanel.SetActive(true);
        if (completionText != null)
        {
            completionText.text = $"{GameData.completedCraftName}が完成しました！";
        }
    }
}