using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    public void OnQuitButtonPressed()
    {
  
        //  GameManager 활용하여 모든 데이터 저장
        var gm = GameManager.Instance;
        gm.PlayerDataManager.SavePlayerData();
        gm.BossDataManager.SaveBossRuntimeData();
        gm.SoundManager.SaveOptionData();

        //  Unity 에디터에서 실행 중이라면 강제 종료 추가
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
