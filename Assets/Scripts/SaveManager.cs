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
            Debug.Log($"파일 저장 완료: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"파일 저장 실패: {ex.Message}");
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
                Debug.Log($"파일 로드 완료: {filePath}");
                return data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"파일 로드 실패: {ex.Message}");
            }
        }
        else
        {
            Debug.Log($"파일이 없습니다. 기본 값 반환: {filePath}");
        }
        return new T(); // 기본 값 반환
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
            Debug.LogWarning($"파일 {fileName}이 존재하지 않습니다. 기본값을 반환합니다.");
            return new T(); // 기본값 생성
        }
    }
}
