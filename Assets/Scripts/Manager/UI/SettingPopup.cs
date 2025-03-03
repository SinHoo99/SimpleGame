using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private AudioMixer AudioMixer => GameManager.Instance.GetAudioMixer();

    [Header("Sound")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        BGMSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SFXSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void OnEnable()
    {
        if (AudioMixer == null)
        {
            Debug.LogError("SettingPopup: AudioMixer가 설정되지 않았습니다!");
            return;
        }
        if (AudioMixer.GetFloat(Mixer.BGM, out float BGMVolume))
        {
            BGMSlider.value = Mathf.Pow(10, (BGMVolume / 20));
        }

        if (AudioMixer.GetFloat(Mixer.SFX, out float SFXVolume))
        {
            SFXSlider.value = Mathf.Pow(10, (SFXVolume / 20));
        }
    }



    private void OnDisable()
    {
        try
        {
            //  Unity Editor에서 Play Mode가 종료되었는지 확인
            if (!Application.isPlaying)
            {
                Debug.LogWarning("⚠️ SettingPopup: Unity Editor에서 Play Mode 종료 중이므로 OnDisable() 실행하지 않음.");
                return;
            }

            //  GameManager가 존재하지 않거나 게임이 종료 중이면 실행하지 않음
            if (GameManager.Instance == null)
            {
                Debug.LogWarning(" SettingPopup: GameManager가 삭제되었거나 초기화되지 않았으므로 OnDisable() 실행하지 않음.");
                return;
            }

            if (GameManager.Instance.isQuitting)
            {
                Debug.LogWarning(" SettingPopup: 게임이 종료 중이므로 OnDisable() 실행하지 않음.");
                return;
            }

            if (GameManager.Instance.GetAudioMixer() == null)
            {
                Debug.LogWarning(" SettingPopup: AudioMixer가 아직 초기화되지 않았으므로 OnDisable() 실행하지 않음.");
                return;
            }

            // 설정 저장 로직 실행 (GameManager가 살아있는 경우에만)
            AudioMixer.GetFloat(Mixer.BGM, out float BGMVolume);
            GameManager.Instance.SoundManager.NowOptionData.BGMVolume = BGMVolume;

            AudioMixer.GetFloat(Mixer.SFX, out float SFXVolume);
            GameManager.Instance.SoundManager.NowOptionData.SFXVolume = SFXVolume;

            GameManager.Instance.SoundManager.SaveOptionData();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($" SettingPopup: OnDisable() 실행 중 예외 발생 - {ex.Message}");
        }
    }


    public void Initializer()
    {
        AudioMixer.SetFloat(Mixer.BGM, GM.SoundManager.NowOptionData.BGMVolume);
        AudioMixer.SetFloat(Mixer.SFX, GM.SoundManager.NowOptionData.SFXVolume);
    }

    public void ChangeBGMVolume(float volume)
    {
        if (volume == 0)
        {
            AudioMixer.SetFloat(Mixer.BGM, -80f);
        }
        else
        {
            AudioMixer.SetFloat(Mixer.BGM, Mathf.Log10(volume) * 20);
        }
    }

    public void ChangeSFXVolume(float volume)
    {
        if (volume == 0)
        {
            AudioMixer.SetFloat(Mixer.SFX, -80f);
        }
        else
        {
            AudioMixer.SetFloat(Mixer.SFX, Mathf.Log10(volume) * 20);
        }
    }

    #region 설정 창 활성화 토글

    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            GameManager.Instance.PlaySFX(SFX.Click);
        }
        else
        {
            gameObject.SetActive(true);
            GameManager.Instance.PlaySFX(SFX.Click);
        }
    }

    #endregion
}
