using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;


    /// 과일 아이템을 업데이트하고 fruitID를 설정하는 함수
    public void UpdateFruit(FruitsID id, int count, Sprite icon)
    {
        fruitID = id;

        // 버튼 이미지 설정
        Image buttonImage = fruitButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }

        // 과일 개수 텍스트 업데이트
        fruitText.text = $"{count}개";
    }


    /// 과일 버튼 클릭 시 실행되는 이벤트
    public void OnFruitButtonClicked()
    {
        GameManager.Instance.UIManager.InventoryManager.FruitUIManager.OnFruitSelected(fruitID);

        // ObjectPool에서 과일 오브젝트 가져오기
        PoolObject objToReturn = FindActiveFruit();

        if (objToReturn != null)
        {
            GameManager.Instance.ObjectPool.ReturnObject(fruitID.ToString(), objToReturn);

            bool success = SubtractFruitAndCalculateCoins(
                fruitID,1
                ,GameManager.Instance.DataManager.FriutDatas
            );

            if (success)
            {
                Debug.Log($"{fruitID} 반환 완료, 코인 획득 및 UI 갱신 완료");
            }
            else
            {
                Debug.LogWarning($"과일 데이터({fruitID})가 존재하지 않아 보상을 지급할 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"{fruitID}에 해당하는 활성화된 오브젝트가 없습니다.");
        }
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
    }
    /// ObjectPool에서 현재 활성화된 과일 오브젝트를 찾아 반환
    private PoolObject FindActiveFruit()
    {
        if (!GameManager.Instance.ObjectPool.PoolDictionary.TryGetValue(fruitID.ToString(), out List<PoolObject> fruitList))
        {
            return null;
        }

        foreach (var obj in fruitList)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }
    public bool SubtractFruitAndCalculateCoins(FruitsID fruitID, int amount, Dictionary<FruitsID, FruitsData> fruitDataDictionary)
    {
        if (!fruitDataDictionary.TryGetValue(fruitID, out var fruitData))
        {
            Debug.LogWarning($"과일 데이터({fruitID})를 찾을 수 없습니다.");
            return false;
        }

        // 과일 수량 감소
        if (GM.PlayerDataManager.NowPlayerData.Inventory.TryGetValue(fruitID, out var collectedFruit))
        {
            collectedFruit.Amount -= amount;
        }
        else
        {
            GM.PlayerDataManager.NowPlayerData.Inventory[fruitID] = new CollectedFruitData { ID = fruitID, Amount = amount };
        }
        return true;
    }
}
