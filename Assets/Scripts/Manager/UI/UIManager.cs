using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager; // Inspector에서 할당
    public InventoryManager InventoryManager => _inventoryManager;

    private GameObject currentActiveUI = null;


    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI 초기화
    }

    public void OnDoTween(GameObject gameObject, Vector3 originalPosition, int targetpositionY)
    {
        bool isVisible = gameObject.activeSelf;

        if (!isVisible)
        {
            if (currentActiveUI != null && currentActiveUI != gameObject)
            {
                HideUI(currentActiveUI);
            }
            // UI 활성화 후 올라가는 애니메이션
            gameObject.SetActive(true);
            gameObject.transform.DOMoveY(targetpositionY, 0.5f).SetEase(Ease.OutCubic);
            currentActiveUI = gameObject;
        }
        else
        {
            HideUI(gameObject);
        }

        isVisible = !isVisible;
    }

    private void HideUI(GameObject uiObject)
    {
        // 원래 위치를 UI의 컴포넌트에서 직접 가져오기
        IShowAndHide uiScript = uiObject.GetComponent<IShowAndHide>();
        if (uiScript is SellingUI sellingUI)
        {
            uiObject.transform.DOMoveY(sellingUI.OriginalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => uiObject.SetActive(false));
        }
        else if (uiScript is DictionaryUI dictionaryUI)
        {
            uiObject.transform.DOMoveY(dictionaryUI.OriginalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => uiObject.SetActive(false));
        }

        if (currentActiveUI == uiObject)
            currentActiveUI = null; // 비활성화되었으므로 초기화
    }

}
