using UnityEngine;
using System.Collections;

public class LootEffect : MonoBehaviour
{
    [Header("動きの設定")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float popHeight = 1.5f;
    [SerializeField] private float popDuration = 0.5f;
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float spreadRange = 1.0f;

    [Header("吸い込み・待機設定")]
    [SerializeField] private float magnetRange = 3.0f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float hoverAmount = 0.1f;

    private ItemData itemData;
    private int quantity;
    private Inventory targetInventory;
    private Transform targetPlayer;

    private AudioSource targetAudioSource;
    private AudioClip pickupSound;

    private bool isReadyToSuck = false;
    private float baseYPos;
    private float initialMoveSpeed;

    public void Initialize(Sprite itemSprite, Vector3 startPos, Transform playerTransform, Inventory inventory, ItemData item, int amount, AudioSource audioSource, AudioClip sound)
    {
        if (spriteRenderer != null) spriteRenderer.sprite = itemSprite;

        transform.position = startPos + Vector3.up * 0.2f;
        targetPlayer = playerTransform;
        targetInventory = inventory;
        itemData = item;
        quantity = amount;

        targetAudioSource = audioSource;
        pickupSound = sound;

        initialMoveSpeed = moveSpeed;

        StartCoroutine(AnimateLoot());
    }

    private IEnumerator AnimateLoot()
    {
        Vector3 startPos = transform.position;
        float randomX = Random.Range(-spreadRange, spreadRange);
        Vector3 dropPoint = startPos + new Vector3(randomX, 0, 0);

        float timer = 0f;
        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / popDuration;

            float height = Mathf.Sin(progress * Mathf.PI) * popHeight;
            Vector3 currentPos = Vector3.Lerp(startPos, dropPoint, progress);
            transform.position = currentPos + Vector3.up * height;

            yield return null;
        }

        baseYPos = transform.position.y;
        isReadyToSuck = true;
    }

    private void Update()
    {
        if (!isReadyToSuck || targetPlayer == null) return;

        float distance = Vector2.Distance(transform.position, targetPlayer.position);

        if (distance < magnetRange)
        {
            float step = moveSpeed * Time.deltaTime;
            moveSpeed += acceleration * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, step);

            if (distance < 0.5f)
            {
                CollectItem();
            }
        }
        else
        {
            moveSpeed = initialMoveSpeed;
            float newY = baseYPos + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void CollectItem()
    {
        if (targetInventory != null && itemData != null)
        {
            bool success = targetInventory.AddItem(itemData, quantity);

            if (success)
            {
                if (targetAudioSource != null && pickupSound != null)
                {
                    targetAudioSource.pitch = Random.Range(0.9f, 1.1f);
                    targetAudioSource.PlayOneShot(pickupSound);
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("インベントリがいっぱいです");
            }
        }
    }
}