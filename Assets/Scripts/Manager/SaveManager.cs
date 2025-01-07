using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using static UnityEditor.Progress;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    [Header("수집목록")]
    public Dictionary<FriutsID, NowFruitsData> Inventory = new Dictionary<FriutsID, NowFruitsData>();
}

[System.Serializable]
public class NowFruitsData
{
    public FriutsID ID;
    public int Amount;
}

public class SaveManager : MonoBehaviour
{
    public void SaveData<T>(T data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
        try
        {
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonData);
            Debug.Log($"[SaveManager] 데이터 저장 성공: {path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] 데이터 저장 실패: {ex.Message}");
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
                Debug.Log($"[SaveManager] 데이터 로드 성공: {path}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] 데이터 로드 실패: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"[SaveManager] 파일이 존재하지 않습니다: {path}");
        }

        data = default(T);
        return false;
    }
}
