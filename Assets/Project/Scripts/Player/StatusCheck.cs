using UnityEngine;
using Cainos.CustomizablePixelCharacter;

public class StatusCheck : MonoBehaviour
{
    private PixelCharacterController controller;
    private Rigidbody2D rb;

    void Start()
    {
        controller = GetComponent<PixelCharacterController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnGUI()
    {
        if (!controller) return;

        // 画面左上にステータスを赤文字で表示
        GUIStyle style = new GUIStyle();
        style.fontSize = 24; // 見やすいサイズ
        style.normal.textColor = Color.red; // 赤色

        GUILayout.BeginArea(new Rect(20, 20, 600, 800));

        // 1. 時間が止まっていないか？
        GUILayout.Label($"Time Scale: {Time.timeScale}", style);

        // 2. 入力は届いているか？
        GUILayout.Label($"Input Move: {controller.inputMove}", style);
        GUILayout.Label($"Input Run: {controller.inputRun}", style);

        // 3. コントローラーの認識は？
        GUILayout.Label($"Is Dead: {controller.IsDead}", style);         // 死んでないか？
        GUILayout.Label($"Is Grounded: {controller.IsGrounded}", style); // 地面についているか？
        GUILayout.Label($"Is Idle: {controller.IsIdle}", style);         // アイドル状態か？
        GUILayout.Label($"Is Running: {controller.IsRunning}", style);   // 走っている認識か？
        GUILayout.Label($"Is LookingAtTarget: {controller.IsLookingAtTarget}", style); // ターゲット注視モードになっていないか？

        // 4. 物理挙動は？
        // Unity 6以降なら linearVelocity、それ以前なら velocity
#if UNITY_6000_0_OR_NEWER
        GUILayout.Label($"RB Velocity: {rb.linearVelocity}", style);
#else
        GUILayout.Label($"RB Velocity: {rb.velocity}", style);
#endif

        GUILayout.EndArea();
    }
}