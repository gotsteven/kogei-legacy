using UnityEngine;
using TMPro;

public class BedroomSceneSetup : MonoBehaviour
{
    public GameObject sleepPanel;
    public TextMeshProUGUI dayText;

    public void OnSleepButtonPressed()
    {
        GlobalGameManager.Instance.StartSleepSequence(sleepPanel, dayText);
    }
}