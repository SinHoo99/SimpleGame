using UnityEngine;
using UnityEngine.UI;

public class HealthStatusUI : MonoBehaviour
{
    public HealthSystem HealthSystem;

    [Header("HP UI")]
    [SerializeField] private Slider HPBar;

    #region �̺�Ʈ ���
    public void SetStatusEvent()
    {
        HealthSystem.OnChangeHP += UpdateHPStatus;
    }
    #endregion

    #region HP ������Ʈ
    public void UpdateHPStatus()
    {
        HPBar.value = HealthSystem.CurHP / HealthSystem.MaxHP;
    }
    #endregion
    public void ShowSlider()
    {
        this.gameObject.SetActive(true);
    }

    public void HideSlider() 
    {
        this.gameObject.SetActive(false);
    }
}
