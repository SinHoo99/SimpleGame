using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI GameoverText;
    [SerializeField] private TextMeshProUGUI ContinueText;
    [SerializeField] private Button ContinueButton;

    private Sequence sequence;

    private void Start()
    {
        StartTween();
    }

    private void StartTween()
    {
        // 시퀀스 생성
        sequence = DOTween.Sequence().SetUpdate(true);

        // 트윈 생성
        Tween gameoverText = GameoverText.DOFade(1f, 1.5f).SetUpdate(true);
        Tween continueText = ContinueText.DOFade(1f, 1.0f).SetUpdate(true);

        // 시퀀스에 트윈 추가
        sequence.Append(gameoverText);
        sequence.Append(continueText);
    }

    public void OnClickContinue()
    {
        GameManager.Instance.PlayerDataManager.DestroyData();
    }
}
