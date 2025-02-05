using UnityEngine;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    public void ShowAndHide()
    {
        this.gameObject.SetActive(!gameObject.activeSelf);
    }
}
