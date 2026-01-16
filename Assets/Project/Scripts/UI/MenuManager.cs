using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    void Start()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);
        }
    }
}