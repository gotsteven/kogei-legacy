using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class FirewoodManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static FirewoodManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI countText;
    public Transform firewoodGridArea; // FirewoodGridAreaを参照
    public Canvas canvas;
    public Sprite firewoodSprite;

    [Header("Settings")]
    public int initialCount = 8;
    public int maxCount = 15;
    public float regenInterval = 3f;

    private int currentCount;
    private float regenTimer = 0f;
    private bool isDragging = false;
    private GameObject dragObject;
    private List<Image> firewoodIcons = new List<Image>();

    public bool IsDragging => isDragging;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // FirewoodGridArea内のすべてのImageを取得
        if (firewoodGridArea != null)
        {
            foreach (Transform child in firewoodGridArea)
            {
                Image icon = child.GetComponent<Image>();
                if (icon != null)
                {
                    firewoodIcons.Add(icon);
                }
            }
        }

        currentCount = initialCount;
        UpdateUI();
    }

    void Update()
    {
        if (currentCount < maxCount)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= regenInterval)
            {
                currentCount++;
                regenTimer = 0f;
                UpdateUI();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        KilnGameManager gm = FindFirstObjectByType<KilnGameManager>();
        if (gm == null || !gm.IsGameActive) return;
        if (currentCount <= 0) return;

        isDragging = true;
        currentCount--;
        UpdateUI();

        // ドラッグオブジェクト作成
        dragObject = new GameObject("DragFirewood");
        dragObject.transform.SetParent(canvas.transform, false);

        var img = dragObject.AddComponent<Image>();

        if (firewoodSprite != null)
        {
            img.sprite = firewoodSprite; // 画像をセット
            img.color = Color.white;     // 色を白（画像そのまま）にする
            img.SetNativeSize();         // 画像本来のサイズにする
        }
        else
        {
            img.color = new Color(0.6f, 0.3f, 0.1f);
            dragObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 50);
        }

        img.raycastTarget = false;

        dragObject.transform.localRotation = Quaternion.Euler(0, 0, -90);

        // ★サイズ調整：もし薪が小さすぎたり大きすぎたらここを変える
        // dragObject.transform.localScale = new Vector3(1.5f, 1.5f, 1f); 

        // マウス位置に配置
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );
        dragObject.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragObject != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out pos
            );
            dragObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;

        bool hitKiln = false;
        if (eventData.pointerEnter != null)
        {
            var kiln = eventData.pointerEnter.GetComponentInParent<Kiln>();

            if (kiln != null)
            {
                kiln.AddFirewood();
                hitKiln = true;
            }
        }

        if (!hitKiln)
        {
            currentCount++;
            UpdateUI();
        }

        if (dragObject != null)
        {
            Destroy(dragObject);
        }
    }

    void UpdateUI()
    {
        // テキスト更新
        if (countText != null)
        {
            countText.text = $"薪: {currentCount}/{maxCount}";
        }

        // 薪アイコンの表示/非表示を更新
        for (int i = 0; i < firewoodIcons.Count; i++)
        {
            if (i < currentCount)
            {
                firewoodIcons[i].enabled = true; // 在庫がある薪は表示
            }
            else
            {
                firewoodIcons[i].enabled = false; // 在庫がない薪は非表示
            }
        }
    }
}
