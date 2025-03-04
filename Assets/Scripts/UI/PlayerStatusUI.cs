using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI BossText;
    private void Start()
    {
        PlayerCoin();
        BossStatus();
    }
    private void OnEnable()
    {
        Debug.Log("[PlayerStatusUI] OnEnable 호출됨. OnBossDefeated 구독 시작");
        Boss.OnBossDefeated += UpdateCoinUI; // 보스 처치 이벤트 구독
    }

    private void OnDisable()
    {
        Debug.Log("[PlayerStatusUI] OnDisable 호출됨. OnBossDefeated 구독 해제");
        Boss.OnBossDefeated -= UpdateCoinUI; // 이벤트 해제
    }


    public void PlayerCoin()
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }

    public void BossStatus()
    {
        BossText.text = $"보스 단계 {GM.BossDataManager.BossRuntimeData.CurrentBossID}";
    }
    private void UpdateCoinUI(int reward)
    {
        Debug.Log($"[PlayerStatusUI] UpdateCoinUI 호출됨. 보상: {reward}, 현재 코인: {GM.PlayerDataManager.NowPlayerData.PlayerCoin}");
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }
}
