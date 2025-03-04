using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager; // Inspector에서 할당
    public InventoryManager InventoryManager => _inventoryManager;

    private GameObject currentActiveUI = null;
    private Vector3 originalPosition; // 기존 UI의 원래 위치 저장
    private float frontZ = -5f;  // UI가 활성화될 때 가장 앞으로 이동할 Z값

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI 초기화
    }

    public void OnDoTween(GameObject uiObject, Vector3 originalPos)
    {
        bool isVisible = uiObject.activeSelf;

        if (!isVisible)
        {
            if (currentActiveUI != null && currentActiveUI != uiObject)
            {
                HideUI(currentActiveUI);
            }

            // 기존 UI의 원래 위치 저장
            originalPosition = uiObject.transform.position;

            // Z값을 조정하여 가장 앞쪽으로 배치
            uiObject.transform.position = new Vector3(originalPosition.x, originalPosition.y, frontZ);

            // 화면 정중앙의 Y좌표 계산
            float targetPositionY = GetUIScreenCenterY(uiObject);

            // UI 활성화 후 올라가는 애니메이션
            uiObject.SetActive(true);
            uiObject.transform.DOMoveY(targetPositionY, 0.5f).SetEase(Ease.OutCubic);
            currentActiveUI = uiObject;
        }
        else
        {
            HideUI(uiObject);
        }
    }

    private void HideUI(GameObject uiObject)
    {
        IShowAndHide uiScript = uiObject.GetComponent<IShowAndHide>();
        if (uiScript is SellingUI sellingUI)
        {
            uiObject.transform.DOMoveY(sellingUI.OriginalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    uiObject.SetActive(false);
                    ResetZPosition(uiObject);
                });
        }
        else if (uiScript is DictionaryUI dictionaryUI)
        {
            uiObject.transform.DOMoveY(dictionaryUI.OriginalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    uiObject.SetActive(false);
                    ResetZPosition(uiObject);
                });
        }

        if (currentActiveUI == uiObject)
            currentActiveUI = null;
    }

    private void ResetZPosition(GameObject uiObject)
    {
        // UI를 원래 위치로 복구 (Z값도 초기 위치로 되돌림)
        uiObject.transform.position = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z);
    }

    private float GetUIScreenCenterY(GameObject uiObject)
    {
        Canvas canvas = uiObject.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("Canvas not found!");
            return Screen.height * 0.5f;
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return Screen.height * 0.5f;
        }
        else
        {
            // Canvas가 Screen Space - Camera 또는 World일 경우
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            return worldCenter.y;
        }
    }
}
