using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

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
        if (fruitButton.TryGetComponent<Image>(out var buttonImage))
        {
            buttonImage.sprite = icon;
        }

        // 과일 개수 텍스트 업데이트
        fruitText.text = $"{count}개 / 판매가격 : {GM.GetFruitsData(id).Price}";
    }

    /// 과일 버튼 클릭 시 실행되는 이벤트
    public void OnFruitButtonClicked()
    {
        GM.UIManager.InventoryManager.FruitUIManager.OnFruitSelected(fruitID);

        // ObjectPool에서 과일 오브젝트 가져오기
        var objToReturn = GM.ObjectPool.FindActiveObject(fruitID.ToString());

        if (objToReturn != null)
        {
            GM.ObjectPool.ReturnObject(fruitID.ToString(), objToReturn);

            if (SubtractFruitAndCalculateCoins(fruitID, 1))
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

        GM.UIManager.InventoryManager.TriggerInventoryUpdate();
    }

    /// 과일 수량 감소 및 코인 계산
    public bool SubtractFruitAndCalculateCoins(FruitsID fruitID, int amount)
    {
        var fruitData = GM.GetFruitsData(fruitID);
        if (fruitData == null)
        {
            Debug.LogWarning($"과일 데이터({fruitID})를 찾을 수 없습니다.");
            return false;
        }

        var collectedFruit = GM.GetCollectedFruitData(fruitID);
        if (collectedFruit.Amount < amount)
        {
            Debug.LogWarning($"{fruitID}의 수량이 부족하여 감소할 수 없습니다.");
            return false;
        }

        collectedFruit.Amount -= amount;
        GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitData.Price;
        return true;
    }
}
