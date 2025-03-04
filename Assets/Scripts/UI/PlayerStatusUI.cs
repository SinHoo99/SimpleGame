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
        Boss.OnBossDefeated += UpdateCoinUI; // ���� óġ �̺�Ʈ ����
    }

    private void OnDisable()
    {
        Boss.OnBossDefeated -= UpdateCoinUI; // �̺�Ʈ ����
    }


    public void PlayerCoin()
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }

    public void BossStatus()
    {
        BossText.text = $"���� �ܰ� {GM.BossDataManager.BossRuntimeData.CurrentBossID}";
    }
    private void UpdateCoinUI(int reward)
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }
}
