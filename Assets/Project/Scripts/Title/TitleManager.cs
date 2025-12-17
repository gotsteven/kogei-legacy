using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要
using UnityEngine.Video; // Video Playerに必要
using UnityEngine.UI; // Buttonに必要
using System.Collections; // コルーチンに必要

public class TitleManager : MonoBehaviour
{
    [Header("動画再生UI")]
    public GameObject videoPlayerUI; // ヒエラルキーの「TutorialVideo_UI」を設定
    public VideoPlayer videoPlayer;   // ↑が持っているVideoPlayerコンポーネントを設定
    public Button startGameButton; // ↑が持っている「ゲームを始める」ボタン

    [Header("タイトル画面のボタン")]
    public Button continueButton;  // 「CONTINUE」ボタン
    public Button newGameButton;   // 「NEW GAME」ボタン

    [Header("遷移先のシーン名")]
    public string sceneToLoad = "Level1_Village"; // 移動先のシーン名


    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        // 1. まず動画UIを非表示にする
        if (videoPlayerUI != null)
        {
            videoPlayerUI.SetActive(false);
        }

        // 2. タイトル画面のボタンに命令を登録する
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnStartButtonPressed);
        }
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(OnStartButtonPressed);
        }

        // 3. 動画UIの「ゲームを始める」ボタンに命令を登録する
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(LoadGameScene);
        }
    }

    // 「CONTINUE」か「NEW GAME」が押された時に呼び出す関数
    public void OnStartButtonPressed()
    {
        // 動画UIを表示する
        if (videoPlayerUI != null)
        {
            videoPlayerUI.SetActive(true);

            // 動画を再生する
            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
        }
    }

    // 「ゲームを始める」ボタンが押された時に呼び出す関数
    public void LoadGameScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}