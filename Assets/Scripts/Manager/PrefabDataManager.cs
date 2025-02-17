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
            string[] saveableTags = { Tag.Apple, Tag.Banana, Tag.Carrot, Tag.Melon };

            foreach (var obj in pool)
            {
                if (obj.gameObject.activeInHierarchy && System.Array.Exists(saveableTags, tag => obj.CompareTag(tag)))
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
        Debug.Log("PrefabData 저장 완료");
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
                else
                {
                    Debug.LogWarning($"Object Pool에서 {cleanKey}을(를) 찾을 수 없습니다.");
                }
            }

            Debug.Log("PrefabData 로드 완료");
        }
        else
        {
            Debug.LogWarning("PrefabData 로드 실패 또는 데이터 없음");
        }
    }
}
