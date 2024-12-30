using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private Dictionary<string, int> fruitCounts; // 과일 개수 관리
    private UIManager uiManager;
    private float timeAccumulator = 0f; // 시간 누적 변수
    private float fruitsPerSecond = 1f; // 초당 증가할 과일 개수

    private void Awake()
    {
        // UIManager 찾기
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager를 찾을 수 없습니다!");
        }

        InitializeFruitCounts(); // 과일 데이터 초기화
        UpdateFruitUI();         // 초기화 후 UI 업데이트
    }

    /// <summary>
    /// 과일 데이터를 초기화합니다.
    /// </summary>
    private void InitializeFruitCounts()
    {
        fruitCounts = GameManager.Instance.GetGameData()?.fruitCounts ?? new Dictionary<string, int>();

        // 초기값 설정
        foreach (var key in new List<string> { "Apple", "Banana", "Melon" })
        {
            if (!fruitCounts.ContainsKey(key))
            {
                fruitCounts[key] = 0;
            }
        }

        Debug.Log("과일 데이터가 초기화되었습니다.");
    }

    private void Update()
    {
        HandleInput();          // 터치나 클릭으로 과일 추가
        IncreaseFruitsOverTime(); // 초당 과일 증가
    }

    /// <summary>
    /// 터치 또는 클릭 입력 처리
    /// </summary>
    private void HandleInput()
    {
        if (IsInputDetected())
        {
            AddFruits(1); // 1개의 랜덤 과일 추가
        }
    }

    /// <summary>
    /// 시간에 따라 과일을 증가시킵니다.
    /// </summary>
    private void IncreaseFruitsOverTime()
    {
        timeAccumulator += Time.deltaTime;

        // 1초마다 과일 추가
        if (timeAccumulator >= 1f)
        {
            int fruitsToAdd = Mathf.FloorToInt(timeAccumulator * fruitsPerSecond);
            AddFruits(fruitsToAdd);
            timeAccumulator -= Mathf.Floor(timeAccumulator); // 누적 시간 정리
        }
    }

    /// <summary>
    /// 지정된 수의 랜덤 과일을 추가합니다.
    /// </summary>
    /// <param name="count">추가할 과일 수</param>
    private void AddFruits(int count)
    {
        if (fruitCounts == null || fruitCounts.Count == 0)
        {
            Debug.LogWarning("과일 데이터가 초기화되지 않았습니다!");
            return;
        }

        List<string> fruitKeys = new List<string>(fruitCounts.Keys);

        for (int i = 0; i < count; i++)
        {
            // 랜덤 과일 선택 및 추가
            string randomFruit = fruitKeys[UnityEngine.Random.Range(0, fruitKeys.Count)];
            fruitCounts[randomFruit]++;
            Debug.Log($"{randomFruit}의 개수가 증가했습니다! 현재 개수: {fruitCounts[randomFruit]}");
        }

        // UI 업데이트
        UpdateFruitUI();
    }

    /// <summary>
    /// UI 업데이트
    /// </summary>
    private void UpdateFruitUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateFruitCountsUI(fruitCounts);
        }
        else
        {
            Debug.LogWarning("UIManager가 설정되지 않아 UI를 업데이트할 수 없습니다.");
        }
    }
    public void ResetUpdater(GameData gameData)
    {
        timeAccumulator = 0f; // 누적 시간 초기화

        if (gameData != null)
        {
            // GameData의 ResetData를 호출하여 데이터 초기화
            gameData.ResetData();
            fruitCounts = gameData.fruitCounts;

            Debug.Log("ScoreUpdater가 GameData를 통해 초기화되었습니다.");
            UpdateFruitUI(); // 초기화된 상태를 UI에 반영
        }
        else
        {
            Debug.LogWarning("GameData가 null이어서 ScoreUpdater를 초기화할 수 없습니다.");
        }
    }

    /// <summary>
    /// 터치 또는 클릭 입력 감지
    /// </summary>
    /// <returns>입력 여부</returns>
    private bool IsInputDetected()
    {
        // 터치 입력 확인
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }

        // 에디터 환경에서 마우스 클릭 확인
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
#endif

        return false;
    }

    public Dictionary<string, int> GetFruitCounts()
    {
        return new Dictionary<string, int>(fruitCounts);
    }
}
