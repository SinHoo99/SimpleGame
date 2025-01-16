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
            FriutDatas.Add(fruitsData.ID, fruitsData);
        }
    }
}
