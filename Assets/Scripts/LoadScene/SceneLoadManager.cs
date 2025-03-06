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
        StartCoroutine(LoadSceneAsync());
    }

    // �� �̵� �� �ش� �޼��� ���
    public static void LoadScene(int sceneNum)
    {
        _nextSceneNumber = sceneNum;
        SceneManager.LoadScene(1);
    }

    private IEnumerator LoadSceneAsync()
    {
        // �ε� UI Ȱ��ȭ
        LoadingUI.SetActive(true);

        yield return null;

        // �ش� ���� �񵿱������� �ε�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nextSceneNumber);
        // �� �ε��� ������ �ڵ����� �Ѿ�� ����
        asyncLoad.allowSceneActivation = false;

        // �ε� ���� ��Ȳ ������Ʈ
        while (!asyncLoad.isDone)
        {
            // ������� 0 ~ 1 ���� ������ ��ȯ
            #region // ����
            // AsyncOperation.progress ���� 0 ~ 1 ������ ������ ������.
            // 0 ~ 0.9�� �ε� ���� ��Ȳ, 0.9 ~ 1�� ���� Ȱ��ȭ�Ǵ� �����̹Ƿ�,
            // 0.9�� �����ָ� ���� ��Ȳ�� 0% ~ 100%���� ��� �ݿ��� �� �ִ�.
            #endregion
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            ProgressBar.value = progress;
            ProgressText.text = $"Loading...{Mathf.FloorToInt(progress * 100)}%";

            // �ε��� �Ϸ�Ǹ� �� Ȱ��ȭ
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
