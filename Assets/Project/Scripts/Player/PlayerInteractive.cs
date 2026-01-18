using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractive : MonoBehaviour
{
    public Inventory inventory;

    [Header("エフェクト設定")]
    public GameObject lootEffectPrefab;

    [Header("サウンド設定")]
    public AudioClip collectSound;
    private AudioSource audioSource;

    private List<ResourceNode> reachableResources = new List<ResourceNode>();

    [Tooltip("採集のクールダウン時間（秒）")]
    public float collectInterval = 0.5f;
    private float nextCollectTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && Time.time >= nextCollectTime)
        {
            nextCollectTime = Time.time + collectInterval;
            TryCollectResource();
        }
    }

    private void TryCollectResource()
    {
        reachableResources.RemoveAll(item => item == null);

        if (reachableResources.Count > 0)
        {
            // 近い順にソート
            reachableResources.Sort((a, b) =>
                Vector2.Distance(transform.position, a.transform.position)
                .CompareTo(Vector2.Distance(transform.position, b.transform.position)));

            foreach (ResourceNode resource in reachableResources)
            {
                if (resource.itemToGive != null)
                {
                    // ResourceNode側の音（叩く音など）を再生
                    if (audioSource != null && resource.popSound != null)
                    {
                        audioSource.pitch = Random.Range(0.9f, 1.1f);
                        audioSource.PlayOneShot(resource.popSound);
                    }

                    SpawnLootEffect(resource);
                }
            }
        }
    }

    private void SpawnLootEffect(ResourceNode resource)
    {
        if (lootEffectPrefab == null) return;

        Sprite itemSprite = resource.itemToGive.icon;
        GameObject effectObj = Instantiate(lootEffectPrefab, resource.transform.position, Quaternion.identity);

        LootEffect effectScript = effectObj.GetComponent<LootEffect>();
        if (effectScript != null)
        {
            effectScript.Initialize(
                itemSprite,
                resource.transform.position,
                this.transform,
                inventory,
                resource.itemToGive,
                resource.quantity,
                audioSource,
                collectSound
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ResourceNode resource = other.GetComponent<ResourceNode>();
        if (resource != null && !reachableResources.Contains(resource))
        {
            reachableResources.Add(resource);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ResourceNode resource = other.GetComponent<ResourceNode>();
        if (resource != null && reachableResources.Contains(resource))
        {
            reachableResources.Remove(resource);
        }
    }
}