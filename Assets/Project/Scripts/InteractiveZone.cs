using UnityEngine;

public class InteractableZone : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite openedChestSprite;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        Debug.Log("インタラクション成功！");

        if (spriteRenderer != null && openedChestSprite != null)
        {
            spriteRenderer.sprite = openedChestSprite;
        }
    }
}