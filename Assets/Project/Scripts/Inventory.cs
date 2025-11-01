// Inventory.cs
using UnityEngine;
using System.Collections.Generic; 

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public bool AddItem(ItemData item, int amount)
    {
        Debug.Log($"インベントリに {item.itemName} を {amount} 個追加しようとしています。");

        foreach (InventorySlot slot in slots)
        {
            if (slot.item != null && slot.item == item && slot.quantity < item.maxStack)
            {
                int spaceLeft = item.maxStack - slot.quantity;

                if (amount <= spaceLeft)
                {
                    slot.SetQuantity(slot.quantity + amount);
                    Debug.Log($"{item.itemName} を {amount} 個スタックしました。");
                    return true; 
                }
                else
                {
                    slot.SetQuantity(item.maxStack); 
                    amount -= spaceLeft; 
                    Debug.Log($"{item.itemName} を {spaceLeft} 個スタック。残り {amount} 個...");
                }
            }
        }

        if (amount > 0)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == null) 
                {
                    if (amount <= item.maxStack)
                    {
                        slot.SetSlot(item, amount);
                        Debug.Log($"{item.itemName} を {amount} 個、新しいスロットに追加しました。");
                        return true; 
                    }
                    else
                    {

                        slot.SetSlot(item, item.maxStack);
                        amount -= item.maxStack;
                        Debug.Log($"{item.itemName} を {item.maxStack} 個、新しいスロットに追加。残り {amount} 個...");
                    }
                }
            }
        }
        
        if (amount > 0)
        {
            Debug.LogWarning($"インベントリが一杯で {item.itemName} を {amount} 個追加できませんでした。");
            return false;
        }

        return true; 
    }
}