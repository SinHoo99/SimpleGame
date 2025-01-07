
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FriutsData
{
    public FriutsID ID;
    public string Name;
    public FriutsType Type;
    public Sprite Image;
    public string Description;
    public int Price;
    public float Probability;
}


public class DataManager : MonoBehaviour
{
    public void Initializer()
    {
        ContainFruitsData();
    }

    public Dictionary<FriutsID, FriutsData> FriutDatas = new Dictionary<FriutsID, FriutsData>();

    public void ContainFruitsData()
    {
        List<Dictionary<string, string>> fruitsDataList = CSVReader.Read(ResourcesPath.FruitsCSV);

        foreach (var datas in fruitsDataList)
        {
            FriutsData fruitsData = new FriutsData();
            fruitsData.ID = (FriutsID)int.Parse(datas[Data.ID]);
            fruitsData.Name = datas[Data.Name];
            fruitsData.Price = int.Parse(datas[Data.Price]);
            fruitsData.Type = (FriutsType)int.Parse(datas[Data.Type]);
            fruitsData.Image = Resources.Load<SpriteAtlas>(ResourcesPath.CSVSprites).GetSprite(datas[Data.Image]);
            fruitsData.Description = datas[Data.Description];
            fruitsData.Probability = float.Parse(datas[Data.Probability]);
            FriutDatas.Add(fruitsData.ID, fruitsData);
        }
    }
}
