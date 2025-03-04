using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager; // Inspector���� �Ҵ�
    public InventoryManager InventoryManager => _inventoryManager;

    private GameObject currentActiveUI = null;
    private Vector3 originalPosition; // ���� UI�� ���� ��ġ ����
    private float frontZ = -5f;  // UI�� Ȱ��ȭ�� �� ���� ������ �̵��� Z��

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI �ʱ�ȭ
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

            // ���� UI�� ���� ��ġ ����
            originalPosition = uiObject.transform.position;

            // Z���� �����Ͽ� ���� �������� ��ġ
            uiObject.transform.position = new Vector3(originalPosition.x, originalPosition.y, frontZ);

            // ȭ�� ���߾��� Y��ǥ ���
            float targetPositionY = GetUIScreenCenterY(uiObject);

            // UI Ȱ��ȭ �� �ö󰡴� �ִϸ��̼�
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
        // UI�� ���� ��ġ�� ���� (Z���� �ʱ� ��ġ�� �ǵ���)
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
            // Canvas�� Screen Space - Camera �Ǵ� World�� ���
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            return worldCenter.y;
        }
    }
}
