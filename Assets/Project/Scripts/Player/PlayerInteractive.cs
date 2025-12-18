using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractive : MonoBehaviour
{
    public Inventory inventory;

    private List<ResourceNode> reachableResources = new List<ResourceNode>();

    [Tooltip("採集のクールダウン時間（秒）")]
    public float collectInterval = 0.5f;
    private float nextCollectTime = 0f;
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
        if (reachableResources.Count > 0)
        {
            ResourceNode resource = reachableResources[0];

            if (resource == null)
            {
                reachableResources.RemoveAt(0);
                return;
            }

            if (inventory != null && resource.itemToGive != null)
            {
                bool addedSuccessfully = inventory.AddItem(resource.itemToGive, resource.quantity);

                if (addedSuccessfully)
                {
                    // reachableResources.Remove(resource); 
                }
                else
                {
                    Debug.Log("インベントリが一杯です！");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ResourceNode resource = other.GetComponent<ResourceNode>();

        if (resource != null)
        {
            if (!reachableResources.Contains(resource))
            {
                reachableResources.Add(resource);
                Debug.Log($"採集可能範囲に入った: {resource.gameObject.name}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ResourceNode resource = other.GetComponent<ResourceNode>();

        if (resource != null && reachableResources.Contains(resource))
        {
            reachableResources.Remove(resource);
            Debug.Log($"採集可能範囲から出た: {resource.gameObject.name}");
        }
    }
}