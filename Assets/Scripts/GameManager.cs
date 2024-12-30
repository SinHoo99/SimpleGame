using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private OfflineScoreUpdater OfflineScoreUpdater;
    private ScoreUpdater ScoreUpdater;
    private UIManager uiManager;
    private GameData gameData;
    private bool isQuitting = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("GameManager 인스턴스가 이미 존재합니다. 중복된 인스턴스를 삭제합니다.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeComponents();

        // Load game data
        gameData = SaveManager.Load<GameData>("GameData.json");

        if (OfflineScoreUpdater != null)
        {
            OfflineScoreUpdater.Initialize(gameData);
        }

        if (uiManager != null)
        {
            uiManager.UpdateFruitCountsUI(gameData.fruitCounts);
        }

    }

    private void InitializeComponents()
    {
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager가 GameManager에 연결되지 않았습니다.");
        }

        OfflineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        if (OfflineScoreUpdater == null)
        {
            Debug.LogError("OfflineScoreUpdater가 GameManager에 연결되지 않았습니다.");
        }

        ScoreUpdater = GetComponent<ScoreUpdater>();
        if (ScoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater GameManager에 연결되지 않았습니다.");
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        if (gameData != null && OfflineScoreUpdater != null)
        {
            gameData.UpdateFruitCounts(OfflineScoreUpdater.GetFruitCounts());
            gameData.lastSaveTime = DateTime.Now.ToString();
            SaveManager.Save("GameData.json", gameData);

            Debug.Log("게임 데이터가 저장되었습니다.");
        }
    }

    public void ResetGame()
    {
        if (gameData != null)
        {
            // ScoreUpdater 초기화
            if (ScoreUpdater != null)
            {
                ScoreUpdater.ResetUpdater(gameData);
                Debug.Log("ScoreUpdater 초기화 완료.");
            }
            else
            {
                Debug.LogWarning("ScoreUpdater가 null입니다.");
            }

            // 데이터 저장
            SaveManager.Save("GameData.json", gameData);
            Debug.Log("초기화된 게임 데이터를 저장했습니다.");

            // UI 업데이트
            if (uiManager != null)
            {
                uiManager.UpdateFruitCountsUI(gameData.fruitCounts);
                Debug.Log("UI가 초기화된 데이터로 갱신되었습니다.");
            }
            else
            {
                Debug.LogWarning("UIManager가 null입니다.");
            }
        }
        else
        {
            Debug.LogWarning("gameData가 null입니다. 초기화를 수행할 수 없습니다.");
        }
    }



    public GameData GetGameData()
    {
        return gameData;
    }
}
