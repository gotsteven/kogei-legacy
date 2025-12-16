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
        // ゲームが開始されていない場合は操作不可
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm == null || !gm.IsGameActive) return;
        if (currentCount <= 0) return;

        isDragging = true;
        currentCount--;
        UpdateUI();

        // ドラッグオブジェクト作成
        dragObject = new GameObject("DragFirewood");
        dragObject.transform.SetParent(canvas.transform, false);

        var img = dragObject.AddComponent<Image>();
        img.color = new Color(0.6f, 0.3f, 0.1f);
        img.raycastTarget = false;

        var rt = dragObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(40, 50);

        // ★重要：作成直後にマウス位置に配置（バグ修正）
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );
        rt.anchoredPosition = pos; // ← これで画面中央に出現しない
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
            var kiln = eventData.pointerEnter.GetComponent<Kiln>();
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
