using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryUI : MonoBehaviour, IShowAndHide
{
    public Vector3 OriginalPosition { get; private set; }

    private void Awake()
    {
        OriginalPosition = transform.position;
    }

    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GameManager.Instance.UIManager.OnDoTween(this.gameObject, OriginalPosition, 820);
    }
}
