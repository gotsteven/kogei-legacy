using UnityEngine;
using System.IO;

/// <summary>
/// ゲームデータのJSON保存・読み込みマネージャー
/// </summary>
public class GameJsonManager : MonoBehaviour
{
    [Header("データベース")]
    [SerializeField] private GameDatabase database;
    
    [Header("ファイル名設定")]
    [SerializeField] private string playerFileName = "players.json";
    [SerializeField] private string inventoryFileName = "inventories.json";
    [SerializeField] private string materialSlotsFileName = "material_slots.json";
    [SerializeField] private string materialItemsFileName = "material_items.json";
    [SerializeField] private string kogeiSlotsFileName = "kogei_slots.json";
    [SerializeField] private string kogeiItemsFileName = "kogei_items.json";
    
    private string DataPath => Application.persistentDataPath;
    
    void Start()
    {
        Debug.Log($"=== データ保存先 ===");
        Debug.Log($"パス: {DataPath}");
        Debug.Log($"==================");
    }
    
    // ========== 全データ保存 ==========
    
    public void SaveAllData()
    {
        SavePlayers();
        SaveInventories();
        SaveMaterialSlots();
        SaveMaterialItems();
        SaveKogeiSlots();
        SaveKogeiItems();
        
        Debug.Log("✅ 全データの保存が完了しました");
    }
    
    // ========== 全データ読み込み ==========
    
    public void LoadAllData()
    {
        LoadPlayers();
        LoadInventories();
        LoadMaterialSlots();
        LoadMaterialItems();
        LoadKogeiSlots();
        LoadKogeiItems();
        
        Debug.Log("✅ 全データの読み込みが完了しました");
    }
    
    // ========== 個別保存メソッド ==========
    
    public void SavePlayers()
    {
        SaveToJson(playerFileName, new PlayerDataList { players = database.GetAllPlayers() });
    }
    
    public void SaveInventories()
    {
        SaveToJson(inventoryFileName, new InventoryDataList { inventories = database.GetAllInventories() });
    }
    
    public void SaveMaterialSlots()
    {
        SaveToJson(materialSlotsFileName, new MaterialInventorySlotList { slots = database.GetAllMaterialSlots() });
    }
    
    public void SaveMaterialItems()
    {
        SaveToJson(materialItemsFileName, new MaterialItemList { items = database.GetAllMaterialItems() });
    }
    
    public void SaveKogeiSlots()
    {
        SaveToJson(kogeiSlotsFileName, new KogeiInventorySlotList { slots = database.GetAllKogeiSlots() });
    }
    
    public void SaveKogeiItems()
    {
        SaveToJson(kogeiItemsFileName, new KogeiItemList { items = database.GetAllKogeiItems() });
    }
    
    // ========== 個別読み込みメソッド ==========
    
    public void LoadPlayers()
    {
        PlayerDataList data = LoadFromJson<PlayerDataList>(playerFileName);
        if (data != null && data.players != null)
        {
            database.ClearPlayerData();
            foreach (var player in data.players)
            {
                database.AddPlayer(player);
            }
            Debug.Log($"プレイヤーデータ読み込み: {data.players.Count}件");
        }
    }
    
    public void LoadInventories()
    {
        InventoryDataList data = LoadFromJson<InventoryDataList>(inventoryFileName);
        if (data != null && data.inventories != null)
        {
            foreach (var inv in data.inventories)
            {
                database.AddInventory(inv);
            }
            Debug.Log($"インベントリデータ読み込み: {data.inventories.Count}件");
        }
    }
    
    public void LoadMaterialSlots()
    {
        MaterialInventorySlotList data = LoadFromJson<MaterialInventorySlotList>(materialSlotsFileName);
        if (data != null && data.slots != null)
        {
            foreach (var slot in data.slots)
            {
                database.AddMaterialSlot(slot);
            }
            Debug.Log($"素材スロットデータ読み込み: {data.slots.Count}件");
        }
    }
    
    public void LoadMaterialItems()
    {
        MaterialItemList data = LoadFromJson<MaterialItemList>(materialItemsFileName);
        if (data != null && data.items != null)
        {
            foreach (var item in data.items)
            {
                database.AddMaterialItem(item);
            }
            Debug.Log($"素材アイテムマスタ読み込み: {data.items.Count}件");
        }
    }
    
    public void LoadKogeiSlots()
    {
        KogeiInventorySlotList data = LoadFromJson<KogeiInventorySlotList>(kogeiSlotsFileName);
        if (data != null && data.slots != null)
        {
            foreach (var slot in data.slots)
            {
                database.AddKogeiSlot(slot);
            }
            Debug.Log($"工芸品スロットデータ読み込み: {data.slots.Count}件");
        }
    }
    
    public void LoadKogeiItems()
    {
        KogeiItemList data = LoadFromJson<KogeiItemList>(kogeiItemsFileName);
        if (data != null && data.items != null)
        {
            foreach (var item in data.items)
            {
                database.AddKogeiItem(item);
            }
            Debug.Log($"工芸品アイテムマスタ読み込み: {data.items.Count}件");
        }
    }
    
    // ========== 汎用保存・読み込み ==========
    
    private void SaveToJson<T>(string fileName, T data)
    {
        try
        {
            string filePath = Path.Combine(DataPath, fileName);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"💾 保存: {fileName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ 保存エラー ({fileName}): {e.Message}");
        }
    }
    
    private T LoadFromJson<T>(string fileName)
    {
        try
        {
            string filePath = Path.Combine(DataPath, fileName);
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"⚠️ ファイルが見つかりません: {fileName}");
                return default(T);
            }
            
            string json = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(json);
            Debug.Log($"📂 読み込み: {fileName}");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ 読み込みエラー ({fileName}): {e.Message}");
            return default(T);
        }
    }
    
    // ========== デバッグ用メソッド ==========
    
    public void CreateSampleData()
    {
        database.ClearAllData();
        
        // プレイヤー作成
        PlayerData player1 = new PlayerData
        {
            playerName = "勇者太郎",
            level = 10,
            exp = 5000
        };
        database.AddPlayer(player1);
        
        // インベントリ作成
        InventoryData inventory1 = new InventoryData
        {
            id = 1,
            playerId = player1.id,
            itemMaxStack = 99
        };
        database.AddInventory(inventory1);
        
        // 素材アイテムマスタ作成
        MaterialItem wood = new MaterialItem("木材", "基本的な建築素材", 1, 99);
        MaterialItem stone = new MaterialItem("石材", "硬い建築素材", 1, 99);
        MaterialItem iron = new MaterialItem("鉄鉱石", "金属の原料", 2, 50);
        
        database.AddMaterialItem(wood);
        database.AddMaterialItem(stone);
        database.AddMaterialItem(iron);
        
        // 素材インベントリスロット作成
        MaterialInventorySlot slot1 = new MaterialInventorySlot
        {
            inventoryId = inventory1.inventoryId,
            itemId = wood.id,
            quantity = 50
        };
        MaterialInventorySlot slot2 = new MaterialInventorySlot
        {
            inventoryId = inventory1.inventoryId,
            itemId = stone.id,
            quantity = 30
        };
        
        database.AddMaterialSlot(slot1);
        database.AddMaterialSlot(slot2);
        
        // 工芸品アイテムマスタ作成
        KogeiItem sword = new KogeiItem("鉄の剣", "基本的な武器", 1, 1);
        KogeiItem shield = new KogeiItem("木の盾", "基本的な防具", 2, 1);
        
        database.AddKogeiItem(sword);
        database.AddKogeiItem(shield);
        
        // 工芸品インベントリスロット作成
        KogeiInventorySlot kSlot1 = new KogeiInventorySlot
        {
            inventoryId = inventory1.inventoryId,
            itemId = sword.id,
            quantity = 1
        };
        
        database.AddKogeiSlot(kSlot1);
        
        Debug.Log("✅ サンプルデータを作成しました");
    }
    
    public void ShowAllData()
    {
        Debug.Log("========== データベース内容 ==========");
        
        // プレイヤー
        var players = database.GetAllPlayers();
        Debug.Log($"--- プレイヤー ({players.Count}件) ---");
        foreach (var p in players)
        {
            Debug.Log($"  {p.playerName} (Lv.{p.level}, EXP:{p.exp})");
        }
        
        // インベントリ
        var inventories = database.GetAllInventories();
        Debug.Log($"--- インベントリ ({inventories.Count}件) ---");
        foreach (var inv in inventories)
        {
            Debug.Log($"  ID:{inv.id}, MaxStack:{inv.itemMaxStack}");
        }
        
        // 素材アイテム
        var materials = database.GetAllMaterialItems();
        Debug.Log($"--- 素材アイテム ({materials.Count}件) ---");
        foreach (var item in materials)
        {
            Debug.Log($"  {item.materialName}: {item.materialDescription}");
        }
        
        // 素材スロット
        var matSlots = database.GetAllMaterialSlots();
        Debug.Log($"--- 素材スロット ({matSlots.Count}件) ---");
        foreach (var slot in matSlots)
        {
            var item = database.GetMaterialItemById(slot.itemId);
            if (item != null)
            {
                Debug.Log($"  {item.materialName} x{slot.quantity}");
            }
        }
        
        // 工芸品アイテム
        var kogeis = database.GetAllKogeiItems();
        Debug.Log($"--- 工芸品アイテム ({kogeis.Count}件) ---");
        foreach (var item in kogeis)
        {
            Debug.Log($"  {item.kogeiName}: {item.kogeiDescription}");
        }
        
        // 工芸品スロット
        var kogeiSlots = database.GetAllKogeiSlots();
        Debug.Log($"--- 工芸品スロット ({kogeiSlots.Count}件) ---");
        foreach (var slot in kogeiSlots)
        {
            var item = database.GetKogeiItemById(slot.itemId);
            if (item != null)
            {
                Debug.Log($"  {item.kogeiName} x{slot.quantity}");
            }
        }
        
        Debug.Log("====================================");
    }
    
    public void DeleteAllJsonFiles()
    {
        DeleteFile(playerFileName);
        DeleteFile(inventoryFileName);
        DeleteFile(materialSlotsFileName);
        DeleteFile(materialItemsFileName);
        DeleteFile(kogeiSlotsFileName);
        DeleteFile(kogeiItemsFileName);
        
        Debug.Log("✅ 全JSONファイルを削除しました");
    }
    
    private void DeleteFile(string fileName)
    {
        string filePath = Path.Combine(DataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"🗑️ 削除: {fileName}");
        }
    }
}