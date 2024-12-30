using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private Dictionary<string, int> fruitCounts; // ���� ���� ����
    private UIManager uiManager;
    private float timeAccumulator = 0f; // �ð� ���� ����
    private float fruitsPerSecond = 1f; // �ʴ� ������ ���� ����

    private void Awake()
    {
        // UIManager ã��
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager�� ã�� �� �����ϴ�!");
        }

        InitializeFruitCounts(); // ���� ������ �ʱ�ȭ
        UpdateFruitUI();         // �ʱ�ȭ �� UI ������Ʈ
    }

    /// <summary>
    /// ���� �����͸� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void InitializeFruitCounts()
    {
        fruitCounts = GameManager.Instance.GetGameData()?.fruitCounts ?? new Dictionary<string, int>();

        // �ʱⰪ ����
        foreach (var key in new List<string> { "Apple", "Banana", "Melon" })
        {
            if (!fruitCounts.ContainsKey(key))
            {
                fruitCounts[key] = 0;
            }
        }

        Debug.Log("���� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    private void Update()
    {
        HandleInput();          // ��ġ�� Ŭ������ ���� �߰�
        IncreaseFruitsOverTime(); // �ʴ� ���� ����
    }

    /// <summary>
    /// ��ġ �Ǵ� Ŭ�� �Է� ó��
    /// </summary>
    private void HandleInput()
    {
        if (IsInputDetected())
        {
            AddFruits(1); // 1���� ���� ���� �߰�
        }
    }

    /// <summary>
    /// �ð��� ���� ������ ������ŵ�ϴ�.
    /// </summary>
    private void IncreaseFruitsOverTime()
    {
        timeAccumulator += Time.deltaTime;

        // 1�ʸ��� ���� �߰�
        if (timeAccumulator >= 1f)
        {
            int fruitsToAdd = Mathf.FloorToInt(timeAccumulator * fruitsPerSecond);
            AddFruits(fruitsToAdd);
            timeAccumulator -= Mathf.Floor(timeAccumulator); // ���� �ð� ����
        }
    }

    /// <summary>
    /// ������ ���� ���� ������ �߰��մϴ�.
    /// </summary>
    /// <param name="count">�߰��� ���� ��</param>
    private void AddFruits(int count)
    {
        if (fruitCounts == null || fruitCounts.Count == 0)
        {
            Debug.LogWarning("���� �����Ͱ� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return;
        }

        List<string> fruitKeys = new List<string>(fruitCounts.Keys);

        for (int i = 0; i < count; i++)
        {
            // ���� ���� ���� �� �߰�
            string randomFruit = fruitKeys[UnityEngine.Random.Range(0, fruitKeys.Count)];
            fruitCounts[randomFruit]++;
            Debug.Log($"{randomFruit}�� ������ �����߽��ϴ�! ���� ����: {fruitCounts[randomFruit]}");
        }

        // UI ������Ʈ
        UpdateFruitUI();
    }

    /// <summary>
    /// UI ������Ʈ
    /// </summary>
    private void UpdateFruitUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateFruitCountsUI(fruitCounts);
        }
        else
        {
            Debug.LogWarning("UIManager�� �������� �ʾ� UI�� ������Ʈ�� �� �����ϴ�.");
        }
    }
    public void ResetUpdater(GameData gameData)
    {
        timeAccumulator = 0f; // ���� �ð� �ʱ�ȭ

        if (gameData != null)
        {
            // GameData�� ResetData�� ȣ���Ͽ� ������ �ʱ�ȭ
            gameData.ResetData();
            fruitCounts = gameData.fruitCounts;

            Debug.Log("ScoreUpdater�� GameData�� ���� �ʱ�ȭ�Ǿ����ϴ�.");
            UpdateFruitUI(); // �ʱ�ȭ�� ���¸� UI�� �ݿ�
        }
        else
        {
            Debug.LogWarning("GameData�� null�̾ ScoreUpdater�� �ʱ�ȭ�� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// ��ġ �Ǵ� Ŭ�� �Է� ����
    /// </summary>
    /// <returns>�Է� ����</returns>
    private bool IsInputDetected()
    {
        // ��ġ �Է� Ȯ��
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }

        // ������ ȯ�濡�� ���콺 Ŭ�� Ȯ��
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
