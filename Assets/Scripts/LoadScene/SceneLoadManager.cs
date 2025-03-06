using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private static int _nextSceneNumber;

    [Header("Load Object")]
    public GameObject LoadingUI;
    public Slider ProgressBar;
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(FakeLoading());
    }

    private IEnumerator FakeLoading()
    {
        LoadingUI.SetActive(true);
        float progress = 0f;

        // 2�� ���� ���� �ٰ� õõ�� ����
        while (progress < 1f)
        {
            progress += Time.deltaTime * 0.5f; // 2�� ���� 100% ����
            ProgressBar.value = progress;
            ProgressText.text = $"Loading... {Mathf.FloorToInt(progress * 100)}%";
            yield return null;
        }

        // �ε� �Ϸ� �� ���� ������ �̵�
        SceneManager.LoadScene("SampleScene");
    }
}
