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
        Debug.Log("[PlayerStatusUI] OnEnable ȣ���. OnBossDefeated ���� ����");
        Boss.OnBossDefeated += UpdateCoinUI; // ���� óġ �̺�Ʈ ����
    }

    private void OnDisable()
    {
        Debug.Log("[PlayerStatusUI] OnDisable ȣ���. OnBossDefeated ���� ����");
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
        Debug.Log($"[PlayerStatusUI] UpdateCoinUI ȣ���. ����: {reward}, ���� ����: {GM.PlayerDataManager.NowPlayerData.PlayerCoin}");
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }
}
