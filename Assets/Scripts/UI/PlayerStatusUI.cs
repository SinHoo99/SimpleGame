using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    public TextMeshProUGUI CoinText;

    private void Start()
    {
        CoinText.text = $"{GameManager.Instance.NowPlayerData.PlayerCoin}";
    }
    public void PlayerCoin()
    {
        CoinText.text = $"{GameManager.Instance.NowPlayerData.PlayerCoin}";
    }
}
