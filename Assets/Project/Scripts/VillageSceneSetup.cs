using UnityEngine;
using TMPro;

public class VillageSceneSetup : MonoBehaviour
{
    [Header("操作する対象")]
    public GameObject player; 
    public GameObject dayLogPanel;
    public TextMeshProUGUI dayLogText;

    void Start()
    {
        // GlobalGameManagerを呼び出す
        if (GlobalGameManager.Instance != null)
        {
            GlobalGameManager.Instance.OnVillageLoaded(player, dayLogPanel, dayLogText);
        }
    }
}