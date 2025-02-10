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
            fruitsData.Prefab = Resources.Load<PoolObject>(datas[Data.Prefab]);
            FriutDatas.Add(fruitsData.ID, fruitsData);
        }
    }

    public GameObject GetFruitPrefab(FruitsID fruitID)
    {
        // 과일 데이터 가져오기
        if (!FriutDatas.TryGetValue(fruitID, out var fruitsData))
        {
            Debug.LogWarning($"{fruitID}에 해당하는 데이터가 없습니다.");
            return null;
        }

        // 이미 로드된 Prefab을 반환 (Resources.Load() 중복 호출 제거)
        if (fruitsData.Prefab == null)
        {
            Debug.LogWarning($"프리팹이 올바르게 로드되지 않았습니다. FruitID: {fruitID}");
            return null;
        }

        return fruitsData.Prefab.gameObject;
    }
}
