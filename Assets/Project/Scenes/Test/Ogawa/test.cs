using UnityEngine;

public class PlayerPersistent : MonoBehaviour
{
    // シングルトン（重複防止）の処理
    // これがないと、元のシーンに戻ったときにプレイヤーが2人に増えてしまいます
    public static PlayerPersistent Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // このGameObject（プレイヤー自身）をシーン遷移で破壊しないようにする
            DontDestroyOnLoad(this.gameObject); 
        }
        else
        {
            // すでにプレイヤーが存在する場合、新しく作られた自分自身を削除する
            Destroy(this.gameObject);
        }
    }
}