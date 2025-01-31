using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DataManager : MonoBehaviour
{
    public void Initializer()
    {
        ContainFruitsData();
    }

    public Dictionary<FruitsID, FruitsData> FriutDatas = new Dictionary<FruitsID, FruitsData>();

    public void ContainFruitsData()
    {
        List<Dictionary<string, string>> fruitsDataList = CSVReader.Read(ResourcesPath.FruitsCSV);

        foreach (var datas in fruitsDataList)
        {
            FruitsData fruitsData = new FruitsData();
            fruitsData.ID = (FruitsID)int.Parse(datas[Data.ID]);
            fruitsData.Name = datas[Data.Name];
            fruitsData.Price = int.Parse(datas[Data.Price]);
            fruitsData.Type = (FruitsType)int.Parse(datas[Data.Type]);
            fruitsData.Image = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.Image]);
            fruitsData.Description = datas[Data.Description];
            fruitsData.Probability = float.Parse(datas[Data.Probability]);
            fruitsData.PrefabPath = datas[Data.PrefabPath];
            FriutDatas.Add(fruitsData.ID, fruitsData);
        }
    }

    public GameObject GetFruitPrefab(FruitsID fruitID)
    {
        // ���� ������ ��������
        if (!FriutDatas.TryGetValue(fruitID, out var fruitsData))
        {
            Debug.LogWarning($"{fruitID}�� �ش��ϴ� �����Ͱ� �����ϴ�.");
            return null;
        }

        // PrefabPath�� ������� ������ �ε�
        var prefab = Resources.Load<GameObject>(fruitsData.PrefabPath);
        if (prefab == null)
        {
            Debug.LogWarning($"������ ��ΰ� �߸��Ǿ����ϴ�: {fruitsData.PrefabPath}");
            return null;
        }

        return prefab;
    }
}
