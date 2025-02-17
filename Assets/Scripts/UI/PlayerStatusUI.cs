using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public TextMeshProUGUI CoinText;

    private void Start()
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }
    public void PlayerCoin()
    {
        CoinText.text = $"{GM.PlayerDataManager.NowPlayerData.PlayerCoin}";
    }
}
