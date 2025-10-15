using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject HoverImage;

    void Start()
    {
        if (HoverImage != null)
        {
            HoverImage.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverImage != null)
        {
            HoverImage.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoverImage != null)
        {
            HoverImage.SetActive(false);
        }
    }
}