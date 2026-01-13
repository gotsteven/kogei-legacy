using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    [Header("ステータス")]
    public int currentDay = 1;
    public string lastNightLog = "";
    public bool isMorning = false;

    [Header("オーディオ")]
    public AudioSource audioSource;
    public AudioClip snoreSound;
    public AudioClip breakSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartSleepSequence(GameObject sleepPanel, TextMeshProUGUI dayText)
    {
        StartCoroutine(SleepCoroutine(sleepPanel, dayText));
    }

    private IEnumerator SleepCoroutine(GameObject sleepPanel, TextMeshProUGUI dayText)
    {
        Debug.Log("盗難システムを開始します");

        currentDay++;
        Debug.Log("Day " + currentDay);

        Image panelImage = null;

        if (sleepPanel != null)
        {
            sleepPanel.SetActive(true);
            panelImage = sleepPanel.GetComponent<Image>();

            if (panelImage != null)
            {
                panelImage.color = new Color(0, 0, 0, 0);
            }
        }

        if (dayText != null)
        {
            dayText.text = "Day " + currentDay;
            dayText.gameObject.SetActive(false);
        }

        float fadeDuration = 1.0f;
        float timer = 0f;

        while (timer < fadeDuration) // 暗転演出
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            if (panelImage != null)
            {
                panelImage.color = new Color(0, 0, 0, alpha);
            }
            yield return null;
        }

        if (panelImage != null) panelImage.color = Color.black;

        if (dayText != null)
        {
            dayText.gameObject.SetActive(true); // テキスト表示
        }

        yield return new WaitForSeconds(0.5f);

        bool isTheft = Random.value > 0.5f;

        if (isTheft)
        {
            // --- 盗難イベント ---
            audioSource.PlayOneShot(breakSound);

            string[] theftLogs = new string[]
            {
                "昨晩、ガラスの割れる音がした。気づいたときにはもう遅かったようだ。\n外に出ると何者かが走っていくのが見えた。\n手に何か持っているように見えた。あれは何だろう？\n起きて作業場に行くと、\nあれ...? \n\n工房のアイテムが盗まれている...",
                "誰かが押し入った形跡がある。\n猫じゃないことは確かだ。\n昔、野球ボールで隣人に家の窓を割った記憶がある。\nでもこんな夜に？ \nそんなことを考えながら作業場に向かう。\n\n保管していたアイテムがいくつか無くなっている...",
                "夜中に物音がして目が覚めた。何事かと思ったが\n気にせず寝てしまった。\n\n朝日に照らされ、目を覚ました。 \n今日の朝食はパンにしよう \nそんなことを考えながら作業場を眺める。 \n\nよく見ると、\n大事な伝統工芸品が見当たらない...！"
            };

            int randomIndex = Random.Range(0, theftLogs.Length);
            lastNightLog = theftLogs[randomIndex];
        }
        else
        {
            // --- 平穏な夜 ---
            audioSource.PlayOneShot(snoreSound);

            string[] peacefulLogs = new string[]
            {
                "昨晩は静かな夜だった。\nアラームの1分前に目が覚める現象、\nあれにそろそろ名前を付けたい。\n朝日は綺麗だ。\n都会より田舎の方がずっと空気が澄んでる。\n\n今日も素晴らしい1日にしよう。",
                "ぐっすりと眠ることができた。\n昨日家を掃除していたら、\n前に住んでいた人が忘れて行ったのだろう。\nクロスワードの雑誌を見つけた。\nほとんど解けていないものばかりだ！ \n後で解いてみよう。 \n\nおっとその前に何か作ろう。",
                "小鳥のさえずりで目が覚めた。\nなんて素敵な朝なんだ。\nただいい日が続いたりすると、何か悪いことが起きるんじゃないかと思ったりする。 \nそういえば昨日腐ったスープ缶を発見した。 \n開けたら最悪だ。\nあんなもの2度と見たくない \n\nさて今日は何を作ろうか？"
            };

            int randomIndex = Random.Range(0, peacefulLogs.Length);
            lastNightLog = peacefulLogs[randomIndex];
        }

        yield return new WaitForSeconds(5.0f);

        isMorning = true;
        GameData.lastExitDirection = "Workshop";
        SceneManager.LoadScene("Level1_Village");
    }

    public void OnVillageLoaded(GameObject player, GameObject panel, TextMeshProUGUI dateText, TextMeshProUGUI logText)
    {
        if (!isMorning) return;

        if (panel != null)
        {
            panel.SetActive(true);
        }

        // 日付を表示
        if (dateText != null)
        {
            dateText.text = $"Day {currentDay}";
        }

        // ログ本文を表示
        if (logText != null)
        {
            logText.text = lastNightLog;
        }

        isMorning = false;
    }
}