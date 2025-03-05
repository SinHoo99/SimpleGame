using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    public void OnQuitButtonPressed()
    {
  
        //  GameManager Ȱ���Ͽ� ��� ������ ����
        var gm = GameManager.Instance;
        gm.PlayerDataManager.SavePlayerData();
        gm.BossDataManager.SaveBossRuntimeData();
        gm.SoundManager.SaveOptionData();

        //  Unity �����Ϳ��� ���� ���̶�� ���� ���� �߰�
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
