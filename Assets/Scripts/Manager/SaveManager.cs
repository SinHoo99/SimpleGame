using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    public void SaveData<T>(T data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
        try
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // ��ȯ ���� ����
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] ������ ���� ����: {ex.Message}");
        }
    }

    public bool TryLoadData<T>(out T data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
        if (File.Exists(path))
        {
            try
            {
                string jsonData = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<T>(jsonData);
                Debug.Log($"[SaveManager] ������ �ε� ����: {path}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] ������ �ε� ����: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[SaveManager] ������ �������� �ʽ��ϴ�: {path}");
        }

        data = default(T);
        return false;
    }
}
