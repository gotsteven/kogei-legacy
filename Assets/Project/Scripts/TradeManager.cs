using UnityEngine;
using TMPro;       

public class TradeManager : MonoBehaviour
{
    [Header("UIパーツ")]
    public TextMeshProUGUI dialogueText; 
    public GameObject buttonGroup; 

    void Start()
    {
        ShopItem item = FindFirstObjectByType<ShopItem>();
        
        if (item != null)
        {
            ShowOffer(item);
        }
    }

    public void ShowOffer(ShopItem item)
    {
        string name = item.GetName();
        int price = item.GetRandomPrice();

        dialogueText.text = $"Mageは{name}を{price}マニーで売ってほしいそうです。";
        
        buttonGroup.SetActive(true);
    }
}