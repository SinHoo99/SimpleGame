using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;


    /// <summary>
    /// 과일 아이템을 업데이트하고 fruitID를 설정하는 함수
    /// </summary>
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


    /// <summary>
    /// 과일 버튼 클릭 시 실행되는 이벤트
    /// </summary>
    public void OnFruitButtonClicked()
    {
        GameManager.Instance.uiManager.OnFruitSelected(fruitID);

        // ObjectPool에서 과일 오브젝트 가져오기
        PoolObject objToReturn = FindActiveFruit();

        if (objToReturn != null)
        {
            GameManager.Instance.objectPool.ReturnObject(fruitID.ToString(), objToReturn);

            bool success = GameManager.Instance.playerDataManager.NowPlayerData.AddFruitAndCalculateCoins(
                fruitID,1
                ,GameManager.Instance.dataManager.FriutDatas
            );

            if (success)
            {
                GameManager.Instance.uiManager.TriggerInventoryUpdate();

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

    }

    /// <summary>
    /// ObjectPool에서 현재 활성화된 과일 오브젝트를 찾아 반환
    /// </summary>
    private PoolObject FindActiveFruit()
    {
        if (!GameManager.Instance.objectPool.PoolDictionary.TryGetValue(fruitID.ToString(), out List<PoolObject> fruitList))
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
}
