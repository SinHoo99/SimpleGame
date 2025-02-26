using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryUI : MonoBehaviour, IShowAndHide
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GameManager.Instance.UIManager.OnDoTween(this.gameObject, originalPosition, 1500);
    }
}
