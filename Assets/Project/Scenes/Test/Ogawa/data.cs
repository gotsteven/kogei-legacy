using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーデータ
/// </summary>
[Serializable]
public class PlayerData
{
    public string id;                    // UUID
    public string playerName;
    public int level;
    public int exp;
    public string updatedAt;            // ISO 8601形式の文字列
    public string deletedAt;            // ISO 8601形式の文字列
    
    public PlayerData()
    {
        id = Guid.NewGuid().ToString();
        updatedAt = DateTime.UtcNow.ToString("o");
        deletedAt = "";
    }
}

/// <summary>
/// インベントリデータ
/// </summary>
[Serializable]
public class InventoryData
{
    public int id;
    public string playerId;             // UUID
    public string inventoryId;          // UUID
    public int itemMaxStack;
    
    public InventoryData()
    {
        inventoryId = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// 素材インベントリスロット
/// </summary>
[Serializable]
public class MaterialInventorySlot
{
    public string id;                   // UUID
    public string inventoryId;          // UUID
    public string slotId;               // UUID
    public string itemId;               // UUID
    public int quantity;
    
    public MaterialInventorySlot()
    {
        id = Guid.NewGuid().ToString();
        slotId = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// 素材アイテムマスタデータ
/// </summary>
[Serializable]
public class MaterialItem
{
    public string id;                   // UUID
    public string materialName;
    public string materialDescription;
    public int materialType;
    public int itemMaxStack;
    
    public MaterialItem()
    {
        id = Guid.NewGuid().ToString();
    }
    
    public MaterialItem(string name, string desc, int type, int maxStack)
    {
        id = Guid.NewGuid().ToString();
        materialName = name;
        materialDescription = desc;
        materialType = type;
        itemMaxStack = maxStack;
    }
}

/// <summary>
/// 工芸品インベントリスロット
/// </summary>
[Serializable]
public class KogeiInventorySlot
{
    public string id;                   // UUID
    public string inventoryId;          // UUID
    public string slotId;               // UUID
    public string itemId;               // UUID
    public int quantity;
    
    public KogeiInventorySlot()
    {
        id = Guid.NewGuid().ToString();
        slotId = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// 工芸品アイテムマスタデータ
/// </summary>
[Serializable]
public class KogeiItem
{
    public string id;                   // UUID
    public string kogeiName;
    public string kogeiDescription;
    public int kogeiType;
    public int kogeiMaxStack;
    
    public KogeiItem()
    {
        id = Guid.NewGuid().ToString();
    }
    
    public KogeiItem(string name, string desc, int type, int maxStack)
    {
        id = Guid.NewGuid().ToString();
        kogeiName = name;
        kogeiDescription = desc;
        kogeiType = type;
        kogeiMaxStack = maxStack;
    }
}

// ========== JSONシリアライズ用ラッパークラス ==========

[Serializable]
public class PlayerDataList
{
    public List<PlayerData> players = new List<PlayerData>();
}

[Serializable]
public class InventoryDataList
{
    public List<InventoryData> inventories = new List<InventoryData>();
}

[Serializable]
public class MaterialInventorySlotList
{
    public List<MaterialInventorySlot> slots = new List<MaterialInventorySlot>();
}

[Serializable]
public class MaterialItemList
{
    public List<MaterialItem> items = new List<MaterialItem>();
}

[Serializable]
public class KogeiInventorySlotList
{
    public List<KogeiInventorySlot> slots = new List<KogeiInventorySlot>();
}

[Serializable]
public class KogeiItemList
{
    public List<KogeiItem> items = new List<KogeiItem>();
}