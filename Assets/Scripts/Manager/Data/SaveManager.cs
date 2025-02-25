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
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 순환 참조 무시
            };

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonData);
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
