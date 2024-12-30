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
            Debug.LogWarning("GameManager �ν��Ͻ��� �̹� �����մϴ�. �ߺ��� �ν��Ͻ��� �����մϴ�.");
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
            Debug.LogError("UIManager�� GameManager�� ������� �ʾҽ��ϴ�.");
        }

        OfflineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        if (OfflineScoreUpdater == null)
        {
            Debug.LogError("OfflineScoreUpdater�� GameManager�� ������� �ʾҽ��ϴ�.");
        }

        ScoreUpdater = GetComponent<ScoreUpdater>();
        if (ScoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater GameManager�� ������� �ʾҽ��ϴ�.");
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

            Debug.Log("���� �����Ͱ� ����Ǿ����ϴ�.");
        }
    }

    public void ResetGame()
    {
        if (gameData != null)
        {
            // ScoreUpdater �ʱ�ȭ
            if (ScoreUpdater != null)
            {
                ScoreUpdater.ResetUpdater(gameData);
                Debug.Log("ScoreUpdater �ʱ�ȭ �Ϸ�.");
            }
            else
            {
                Debug.LogWarning("ScoreUpdater�� null�Դϴ�.");
            }

            // ������ ����
            SaveManager.Save("GameData.json", gameData);
            Debug.Log("�ʱ�ȭ�� ���� �����͸� �����߽��ϴ�.");

            // UI ������Ʈ
            if (uiManager != null)
            {
                uiManager.UpdateFruitCountsUI(gameData.fruitCounts);
                Debug.Log("UI�� �ʱ�ȭ�� �����ͷ� ���ŵǾ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("UIManager�� null�Դϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("gameData�� null�Դϴ�. �ʱ�ȭ�� ������ �� �����ϴ�.");
        }
    }



    public GameData GetGameData()
    {
        return gameData;
    }
}
