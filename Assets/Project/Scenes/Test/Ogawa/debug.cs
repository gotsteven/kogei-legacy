using UnityEngine;
using UnityEditor;

/// <summary>
/// GameJsonManagerのカスタムエディター
/// </summary>
[CustomEditor(typeof(GameJsonManager))]
public class GameJsonManagerEditor : Editor
{
    private bool showDataSection = true;
    private bool showSaveSection = true;
    private bool showLoadSection = true;
    private bool showDeleteSection = false;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GameJsonManager manager = (GameJsonManager)target;
        
        EditorGUILayout.Space(15);
        
        // ========== データ操作セクション ==========
        showDataSection = EditorGUILayout.BeginFoldoutHeaderGroup(showDataSection, "📝 データ操作");
        if (showDataSection)
        {
            EditorGUILayout.BeginVertical("box");
            
            if (GUILayout.Button("✨ サンプルデータを作成", GUILayout.Height(35)))
            {
                manager.CreateSampleData();
            }
            
            EditorGUILayout.Space(5);
            
            if (GUILayout.Button("👁️ データベース内容を表示", GUILayout.Height(35)))
            {
                manager.ShowAllData();
            }
            
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(10);
        
        // ========== 保存セクション ==========
        showSaveSection = EditorGUILayout.BeginFoldoutHeaderGroup(showSaveSection, "💾 保存操作");
        if (showSaveSection)
        {
            EditorGUILayout.BeginVertical("box");
            
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("💾 全データを保存", GUILayout.Height(40)))
            {
                manager.SaveAllData();
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("個別保存", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Players", GUILayout.Height(25)))
            {
                manager.SavePlayers();
            }
            if (GUILayout.Button("Inventories", GUILayout.Height(25)))
            {
                manager.SaveInventories();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Material Items", GUILayout.Height(25)))
            {
                manager.SaveMaterialItems();
            }
            if (GUILayout.Button("Material Slots", GUILayout.Height(25)))
            {
                manager.SaveMaterialSlots();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Kogei Items", GUILayout.Height(25)))
            {
                manager.SaveKogeiItems();
            }
            if (GUILayout.Button("Kogei Slots", GUILayout.Height(25)))
            {
                manager.SaveKogeiSlots();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(10);
        
        // ========== 読み込みセクション ==========
        showLoadSection = EditorGUILayout.BeginFoldoutHeaderGroup(showLoadSection, "📂 読み込み操作");
        if (showLoadSection)
        {
            EditorGUILayout.BeginVertical("box");
            
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("📂 全データを読み込み", GUILayout.Height(40)))
            {
                manager.LoadAllData();
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("個別読み込み", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Players", GUILayout.Height(25)))
            {
                manager.LoadPlayers();
            }
            if (GUILayout.Button("Inventories", GUILayout.Height(25)))
            {
                manager.LoadInventories();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Material Items", GUILayout.Height(25)))
            {
                manager.LoadMaterialItems();
            }
            if (GUILayout.Button("Material Slots", GUILayout.Height(25)))
            {
                manager.LoadMaterialSlots();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Kogei Items", GUILayout.Height(25)))
            {
                manager.LoadKogeiItems();
            }
            if (GUILayout.Button("Kogei Slots", GUILayout.Height(25)))
            {
                manager.LoadKogeiSlots();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(10);
        
        // ========== 削除セクション ==========
        showDeleteSection = EditorGUILayout.BeginFoldoutHeaderGroup(showDeleteSection, "🗑️ 削除操作");
        if (showDeleteSection)
        {
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.HelpBox("⚠️ 削除操作は取り消せません", MessageType.Warning);
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("🗑️ 全JSONファイルを削除", GUILayout.Height(35)))
            {
                if (EditorUtility.DisplayDialog("確認", 
                    "すべてのJSONファイルを削除しますか?\nこの操作は取り消せません。", 
                    "削除", "キャンセル"))
                {
                    manager.DeleteAllJsonFiles();
                }
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(15);
        
        // ========== 使い方ガイド ==========
        EditorGUILayout.HelpBox(
            "📖 使い方ガイド\n\n" +
            "1️⃣ サンプルデータを作成\n" +
            "2️⃣ データベース内容を確認\n" +
            "3️⃣ 全データを保存してJSONファイル作成\n" +
            "4️⃣ 全データを読み込んで動作確認\n\n" +
            "💡 個別保存・読み込みで特定のテーブルのみ操作可能",
            MessageType.Info);
    }
}