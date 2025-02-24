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
    public void PlayerCoin()
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }

    public void BossStatus()
    {
        BossText.text = $"보스 단계 {GM.BossDataManager.BossRuntimeData.CurrentBossID}";
    }
}
