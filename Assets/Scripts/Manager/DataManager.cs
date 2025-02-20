using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DataManager : MonoBehaviour
{
    public void Initializer()
    {
        ContainFruitsData();
        ContainBossData();
    }

    #region 과일 데이터 
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
    #endregion

    #region 보스 데이터 
    public Dictionary<BossID, BossData> BossDatas = new Dictionary<BossID, BossData>();
    
    public void ContainBossData()
    {
        List<Dictionary<string, string>> bossDataList = CSVReader.Read(ResourcesPath.BossCSV);

        foreach (var datas in bossDataList)
        {
            BossData bossData = new BossData();
            bossData.ID = (BossID)int.Parse(datas[Data.ID]);
            bossData.MaxHealth = int.Parse(datas[Data.MaxHealth]);
            bossData.AnimationState = datas[Data.AnimationState];
            if (!BossDatas.ContainsKey(bossData.ID))
            {
                BossDatas.Add(bossData.ID, bossData);
            }
        }
    }

    #endregion
}
