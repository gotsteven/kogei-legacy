using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemData item;
    public int quantity;

    public Image iconImage; 
    
    public TextMeshProUGUI quantityText;

    void Awake()
    {
        if (iconImage == null)
        {
            iconImage = transform.Find("Icon_Image").GetComponent<Image>();
        }

        ClearSlot();
    }

    public void SetSlot(ItemData newItem, int newAmount)
    {
        item = newItem;
        quantity = newAmount;

        iconImage.sprite = item.icon;
        iconImage.enabled = true; 

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        
        iconImage.sprite = null;
        iconImage.enabled = false; 

        quantityText.text = "";
        quantityText.enabled = false;
    }

    public void SetQuantity(int newAmount)
    {
        quantity = newAmount;
        quantityText.text = quantity.ToString();
    }
}