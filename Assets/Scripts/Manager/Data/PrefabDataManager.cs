using System.Collections.Generic;
using UnityEngine;

public class PrefabDataManager
{
    private GameManager GM => GameManager.Instance;

    public void SavePrefabData()
    {
        List<PrefabData> prefabDataList = new List<PrefabData>();

        foreach (var pool in GM.ObjectPool.PoolDictionary.Values)
        {
            int unitLayer = LayerMask.NameToLayer(Layer.Unit);

            foreach (var obj in pool)
            {
                if (obj.gameObject.activeInHierarchy && obj.gameObject.layer == unitLayer)
                {
                    prefabDataList.Add(new PrefabData(
                        obj.name.Replace("(Clone)", "").Trim(),
                        obj.transform.position,
                        obj.transform.rotation
                    ));
                }
            }
        }

        GM.SaveManager.SaveData(prefabDataList);
    }

    public void LoadPrefabData()
    {
        if (GM.SaveManager.TryLoadData(out List<PrefabData> prefabDataList))
        {
            foreach (var prefabData in prefabDataList)
            {
                string cleanKey = prefabData.prefabName.Trim();

                PoolObject obj = GM.ObjectPool.SpawnFromPool(cleanKey);

                if (obj != null)
                {
                    obj.transform.position = prefabData.position.ToVector3();
                    obj.transform.rotation = prefabData.rotation.ToQuaternion();
                    obj.gameObject.SetActive(true);
                }
            }
        }

    }
}
