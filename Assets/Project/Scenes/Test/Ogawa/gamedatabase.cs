using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ゲームの全データを管理するメインデータベース
/// </summary>
[CreateAssetMenu(fileName = "GameDatabase", menuName = "Database/Game Database")]
public class GameDatabase : ScriptableObject
{
    [Header("プレイヤーデータ")]
    [SerializeField] private List<PlayerData> players = new List<PlayerData>();
    
    [Header("インベントリデータ")]
    [SerializeField] private List<InventoryData> inventories = new List<InventoryData>();
    
    [Header("素材インベントリスロット")]
    [SerializeField] private List<MaterialInventorySlot> materialSlots = new List<MaterialInventorySlot>();
    
    [Header("素材アイテムマスタ")]
    [SerializeField] private List<MaterialItem> materialItems = new List<MaterialItem>();
    
    [Header("工芸品インベントリスロット")]
    [SerializeField] private List<KogeiInventorySlot> kogeiSlots = new List<KogeiInventorySlot>();
    
    [Header("工芸品アイテムマスタ")]
    [SerializeField] private List<KogeiItem> kogeiItems = new List<KogeiItem>();
    
    // ========== プレイヤー操作 ==========
    
    public List<PlayerData> GetAllPlayers() => players;
    
    public PlayerData GetPlayerById(string id)
    {
        return players.FirstOrDefault(p => p.id == id);
    }
    
    public void AddPlayer(PlayerData player)
    {
        if (GetPlayerById(player.id) == null)
        {
            players.Add(player);
            Debug.Log($"プレイヤー追加: {player.playerName} (Lv.{player.level})");
        }
        else
        {
            Debug.LogWarning($"プレイヤーID {player.id} は既に存在します");
        }
    }
    
    public void UpdatePlayer(PlayerData updatedPlayer)
    {
        PlayerData existing = GetPlayerById(updatedPlayer.id);
        if (existing != null)
        {
            int index = players.IndexOf(existing);
            players[index] = updatedPlayer;
            Debug.Log($"プレイヤー更新: {updatedPlayer.playerName}");
        }
    }
    
    public void RemovePlayer(string id)
    {
        PlayerData player = GetPlayerById(id);
        if (player != null)
        {
            players.Remove(player);
            Debug.Log($"プレイヤー削除: {player.playerName}");
        }
    }
    
    // ========== インベントリ操作 ==========
    
    public List<InventoryData> GetAllInventories() => inventories;
    
    public List<InventoryData> GetInventoriesByPlayerId(string playerId)
    {
        return inventories.Where(inv => inv.playerId == playerId).ToList();
    }
    
    public InventoryData GetInventoryById(string inventoryId)
    {
        return inventories.FirstOrDefault(inv => inv.inventoryId == inventoryId);
    }
    
    public void AddInventory(InventoryData inventory)
    {
        inventories.Add(inventory);
        Debug.Log($"インベントリ追加: ID={inventory.inventoryId}");
    }
    
    // ========== 素材アイテムマスタ操作 ==========
    
    public List<MaterialItem> GetAllMaterialItems() => materialItems;
    
    public MaterialItem GetMaterialItemById(string id)
    {
        return materialItems.FirstOrDefault(item => item.id == id);
    }
    
    public MaterialItem GetMaterialItemByName(string name)
    {
        return materialItems.FirstOrDefault(item => item.materialName == name);
    }
    
    public void AddMaterialItem(MaterialItem item)
    {
        if (GetMaterialItemById(item.id) == null)
        {
            materialItems.Add(item);
            Debug.Log($"素材アイテム追加: {item.materialName}");
        }
    }
    
    // ========== 工芸品アイテムマスタ操作 ==========
    
    public List<KogeiItem> GetAllKogeiItems() => kogeiItems;
    
    public KogeiItem GetKogeiItemById(string id)
    {
        return kogeiItems.FirstOrDefault(item => item.id == id);
    }
    
    public KogeiItem GetKogeiItemByName(string name)
    {
        return kogeiItems.FirstOrDefault(item => item.kogeiName == name);
    }
    
    public void AddKogeiItem(KogeiItem item)
    {
        if (GetKogeiItemById(item.id) == null)
        {
            kogeiItems.Add(item);
            Debug.Log($"工芸品アイテム追加: {item.kogeiName}");
        }
    }
    
    // ========== 素材インベントリスロット操作 ==========
    
    public List<MaterialInventorySlot> GetAllMaterialSlots() => materialSlots;
    
    public List<MaterialInventorySlot> GetMaterialSlotsByInventoryId(string inventoryId)
    {
        return materialSlots.Where(slot => slot.inventoryId == inventoryId).ToList();
    }
    
    public MaterialInventorySlot GetMaterialSlotById(string slotId)
    {
        return materialSlots.FirstOrDefault(slot => slot.id == slotId);
    }
    
    public void AddMaterialSlot(MaterialInventorySlot slot)
    {
        materialSlots.Add(slot);
        MaterialItem item = GetMaterialItemById(slot.itemId);
        if (item != null)
        {
            Debug.Log($"素材スロット追加: {item.materialName} x{slot.quantity}");
        }
    }
    
    public void UpdateMaterialSlot(MaterialInventorySlot updatedSlot)
    {
        MaterialInventorySlot existing = GetMaterialSlotById(updatedSlot.id);
        if (existing != null)
        {
            int index = materialSlots.IndexOf(existing);
            materialSlots[index] = updatedSlot;
        }
    }
    
    public void RemoveMaterialSlot(string slotId)
    {
        MaterialInventorySlot slot = GetMaterialSlotById(slotId);
        if (slot != null)
        {
            materialSlots.Remove(slot);
        }
    }
    
    // ========== 工芸品インベントリスロット操作 ==========
    
    public List<KogeiInventorySlot> GetAllKogeiSlots() => kogeiSlots;
    
    public List<KogeiInventorySlot> GetKogeiSlotsByInventoryId(string inventoryId)
    {
        return kogeiSlots.Where(slot => slot.inventoryId == inventoryId).ToList();
    }
    
    public KogeiInventorySlot GetKogeiSlotById(string slotId)
    {
        return kogeiSlots.FirstOrDefault(slot => slot.id == slotId);
    }
    
    public void AddKogeiSlot(KogeiInventorySlot slot)
    {
        kogeiSlots.Add(slot);
        KogeiItem item = GetKogeiItemById(slot.itemId);
        if (item != null)
        {
            Debug.Log($"工芸品スロット追加: {item.kogeiName} x{slot.quantity}");
        }
    }
    
    public void UpdateKogeiSlot(KogeiInventorySlot updatedSlot)
    {
        KogeiInventorySlot existing = GetKogeiSlotById(updatedSlot.id);
        if (existing != null)
        {
            int index = kogeiSlots.IndexOf(existing);
            kogeiSlots[index] = updatedSlot;
        }
    }
    
    public void RemoveKogeiSlot(string slotId)
    {
        KogeiInventorySlot slot = GetKogeiSlotById(slotId);
        if (slot != null)
        {
            kogeiSlots.Remove(slot);
        }
    }
    
    // ========== ユーティリティ ==========
    
    public void ClearAllData()
    {
        players.Clear();
        inventories.Clear();
        materialSlots.Clear();
        materialItems.Clear();
        kogeiSlots.Clear();
        kogeiItems.Clear();
        Debug.Log("全データをクリアしました");
    }
    
    public void ClearPlayerData()
    {
        players.Clear();
        inventories.Clear();
        materialSlots.Clear();
        kogeiSlots.Clear();
        Debug.Log("プレイヤーデータをクリアしました");
    }
}