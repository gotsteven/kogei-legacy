using UnityEngine;

public class VictionaryManager : MonoBehaviour
{
    [SerializeField] private GameObject moreInfo;

    void Start()
    {
        if (moreInfo != null)
        {
            moreInfo.SetActive(false);
        }
    }

    public void ToggleMenu()
    {
        if (moreInfo != null)
        {
            bool isActive = moreInfo.activeSelf;
            moreInfo.SetActive(!isActive);
        }
    }
}