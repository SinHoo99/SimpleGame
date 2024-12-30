using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public Dictionary<string, int> fruitCounts;
    public string lastSaveTime;

    public GameData()
    {
        ResetData();
    }

    public void ResetData()
    {
        fruitCounts = new Dictionary<string, int>
        {
            { "Apple", 0 },
            { "Banana", 0 },
            { "Melon", 0 }
        };
        lastSaveTime = DateTime.Now.ToString();

        Debug.Log("GameData�� �⺻������ �ʱ�ȭ�Ǿ����ϴ�.");
    }

    public void UpdateFruitCounts(Dictionary<string, int> dictionary)
    {
        if (dictionary != null)
        {
            fruitCounts = new Dictionary<string, int>(dictionary);
        }
        else
        {
            fruitCounts = new Dictionary<string, int>();
        }
    }
}
