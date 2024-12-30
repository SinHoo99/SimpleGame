using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void Save<T>(string fileName, T data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
            Debug.Log($"���� ���� �Ϸ�: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"���� ���� ����: {ex.Message}");
        }
    }

    public static T Load<T>(string fileName) where T : new()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                T data = JsonUtility.FromJson<T>(jsonData);
                Debug.Log($"���� �ε� �Ϸ�: {filePath}");
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"���� �ε� ����: {ex.Message}");
            }
        }
        else
        {
            Debug.Log($"������ �����ϴ�. �⺻ �� ��ȯ: {filePath}");
        }
        return new T(); // �⺻ �� ��ȯ
    }

    public static T LoadOrDefault<T>(string fileName) where T : new()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogWarning($"���� {fileName}�� �������� �ʽ��ϴ�. �⺻���� ��ȯ�մϴ�.");
            return new T(); // �⺻�� ����
        }
    }
}
