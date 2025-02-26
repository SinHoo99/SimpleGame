using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InventoryManager _inventoryManager; // Inspector���� �Ҵ�
    public InventoryManager InventoryManager => _inventoryManager;

    private GameObject currentActiveUI = null;


    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI �ʱ�ȭ
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
            // UI Ȱ��ȭ �� �ö󰡴� �ִϸ��̼�
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
        // ���� ��ġ�� UI�� ������Ʈ���� ���� ��������
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
            currentActiveUI = null; // ��Ȱ��ȭ�Ǿ����Ƿ� �ʱ�ȭ
    }

}
