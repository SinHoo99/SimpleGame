using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    public void OnResetButtonPressed()
    {
        GameManager.Instance.DestroyData(); // 데이터 초기화
        Debug.Log("Inventory가 초기화되었습니다.");
        GameManager.Instance.SpawnManager.ReturnAllFruitsToPool();
    }
}
