using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要
using UnityEngine.Video; // Video Playerを扱うために必要
using System.Collections; // コルーチンを使うために必要

// クラス名はファイル名と完全に一致させる
public class WorkshopBedInteraction : MonoBehaviour
{
    // === インスペクター（Unityエディタ）で設定する項目 ===
    [Header("操作するUIパネル")]
    public GameObject AnnouncePanelUI;      // 「はい」「いいえ」のパネル
    public Image FadePanel;                 // 暗転用の黒い幕
    public GameObject nextButton;

    [Header("動画再生用の設定")]
    public GameObject VideoDisplay;         // 動画を映すRawImage
    public VideoPlayer videoPlayer;         // Video Playerコンポーネント

    [Header("演出の設定")]
    public float fadeDuration = 1.0f;       // 暗転にかかる時間（1秒）


    // === ベッドがクリックされた時に呼ばれる処理 ===
    // このスクリプトがアタッチされたオブジェクトがクリックされた時に自動で呼ばれる
    private void OnMouseDown()
    {
        // AnnouncePanelUIが設定されているか確認
        if (AnnouncePanelUI != null)
        {
            // パネルを「表示」する
            AnnouncePanelUI.SetActive(true);
            Debug.Log("ベッドがクリックされました！");
        }
        else
        {
            Debug.LogWarning("Announce Panel UIが設定されていません！インスペクターで設定してください。", this);
        }
    }

    // === ここから下は、UIボタンから呼び出すための関数 ===

    // 「はい」ボタンから呼び出す関数
    public void GoToSleep()
    {
        // 安全確認
        if (AnnouncePanelUI == null || FadePanel == null || VideoDisplay == null || videoPlayer == null)
        {
            Debug.LogError("BedInteractionのインスペクタ設定が足りません！");
            return; // 処理を中断
        }

        AnnouncePanelUI.SetActive(false); // 先にパネルを消す
        StartCoroutine(SleepSequence());  // 演出（コルーチン）を開始
    }

    // 「閉じる」ボタンから呼び出す関数
    public void CloseAnnouncePanel()
    {
        if (AnnouncePanelUI != null)
        {
            AnnouncePanelUI.SetActive(false); // パネルを非表示にする
        }
    }


    // === 実際の演出処理（コルーチン） ===

    // 暗転 → 動画再生 の流れを実行する
    private IEnumerator SleepSequence()
    {
        // 1. 黒い幕をフワッと表示（暗転）
        yield return StartCoroutine(Fade(FadePanel, 0f, 1f, fadeDuration));

        // 2. 動画の準備
        VideoDisplay.SetActive(true); // 動画表示用のRawImageをオン
        videoPlayer.Play(); // 動画再生開始

        // 3. 動画が再生されるのを待つ（一瞬）
        yield return new WaitForSeconds(0.1f);

        // 4. 黒い幕をフワッと消す（動画が見え始める）
        yield return StartCoroutine(Fade(FadePanel, 1f, 0f, fadeDuration));

        // 5. 動画が終了するまで待つ
        // (VideoPlayerのLoop設定がオフになっていることを確認)
        yield return new WaitUntil(() => videoPlayer.isPlaying == false);

        // 6. 再び黒い幕をフワッと表示（動画が終わって暗転）
        yield return StartCoroutine(Fade(FadePanel, 0f, 1f, fadeDuration));

        Debug.Log("動画が終了しました。「次に進む」ボタンを表示します。");
        if (nextButton != null)
        {
            nextButton.SetActive(true); // ★★★ ボタンを表示！ ★★★
        }
    }

    // Alpha値をAからBへ、指定した時間で変更する汎用コルーチン
    private IEnumerator Fade(Image targetImage, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            Color newColor = targetImage.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, progress);
            targetImage.color = newColor;

            yield return null;
        }

        Color finalColor = targetImage.color;
        finalColor.a = endAlpha;
        targetImage.color = finalColor;
    }
}