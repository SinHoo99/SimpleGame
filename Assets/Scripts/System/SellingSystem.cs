using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    public void OnResetButtonPressed()
    {
        GameManager.Instance.PlayerDataManager.DestroyData(); // ������ �ʱ�ȭ
        Debug.Log("Inventory�� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
