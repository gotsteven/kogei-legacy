using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TradeManager : MonoBehaviour
{
    [Header("UIパーツ")]
    public TextMeshProUGUI dialogueText;
    public GameObject buttonGroup;

    [Header("棚の表示オブジェクト（8個まで設定可）")]
    [SerializeField] private SpriteRenderer[] shelfDisplaySpots;

    [Header("工芸品の最大表示サイズ")]
    [SerializeField] private float targetDisplaySize = 1.5f;

    private KogeiData currentTargetData;     
    private GameObject currentTargetObject;  
    private int currentOfferPrice;           
    private int haggleCount = 0;

    void Start()
    {
        RefreshShelvesAndOffer();
    }

    void RefreshShelvesAndOffer()
    {
        haggleCount = 0;

        foreach (var spot in shelfDisplaySpots)
        {
            if (spot != null) spot.gameObject.SetActive(false);
        }

        currentTargetData = null;
        currentTargetObject = null;

        int maxSpots = shelfDisplaySpots.Length;
        int totalWorks = CraftStorage.works.Count;

        for (int i = 0; i < maxSpots; i++)
        {
            int workIndex = totalWorks - 1 - i;

            if (workIndex >= 0)
            {
                GameObject obj = CraftStorage.works[workIndex];
                if (obj != null)
                {
                    KogeiItem item = obj.GetComponent<KogeiItem>();
                    if (item != null && item.data != null)
                    {
                        shelfDisplaySpots[i].sprite = item.data.artworkImage;
                        shelfDisplaySpots[i].gameObject.SetActive(true);
                        
                        AdjustSpriteSize(shelfDisplaySpots[i], item.data.artworkImage);

                        if (i == 0)
                        {
                            currentTargetData = item.data;
                            currentTargetObject = obj;
                        }
                    }
                }
            }
        }

        if (currentTargetData != null)
        {
            int initialPrice = Random.Range(currentTargetData.minPrice, currentTargetData.maxPrice + 1);
            ShowOffer(currentTargetData.workName, initialPrice);
        }
        else
        {
            dialogueText.text = "工芸品を作ってきてくれないかい？";
            buttonGroup.SetActive(false);
        }
    }

    void ShowOffer(string name, int price)
    {
        currentOfferPrice = price; 
        dialogueText.text = $"Mageは{name}を{price}マニーで売ってほしいそうです。";
        buttonGroup.SetActive(true);
    }

    void AdjustSpriteSize(SpriteRenderer spot, Sprite sprite)
    {
        if (sprite == null) return;
        Vector3 spriteSize = sprite.bounds.size;
        float maxSide = Mathf.Max(spriteSize.x, spriteSize.y);
        if (maxSide > 0)
        {
            float scaleFactor = targetDisplaySize / maxSide;
            spot.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    public void OnOkButton()
    {
        if (currentTargetObject != null)
        {
            CraftStorage.works.Remove(currentTargetObject);
            Destroy(currentTargetObject);

            dialogueText.text = "ありがとう！大事にするよ。";
            buttonGroup.SetActive(false);

            if(shelfDisplaySpots.Length > 0 && shelfDisplaySpots[0] != null)
            {
                shelfDisplaySpots[0].gameObject.SetActive(false);
            }
        }
    }

    public void OnNoButton()
    {
        if (currentTargetData != null)
        {
            haggleCount++;

            if (haggleCount <= 2)
            {
                int increaseAmount = Random.Range(50, 150);
                int newPrice = currentOfferPrice + increaseAmount;
                currentOfferPrice = newPrice;

                dialogueText.text = $"そしたら {newPrice} マニーでどうだい？";
            }
            else
            {
                dialogueText.text = "じゃあ今回は縁がなかったということで...";
                buttonGroup.SetActive(false);
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}