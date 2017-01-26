using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour 
{
    private static AudioManager _instance;
    public static AudioManager Get()
    {
        return _instance;
    }

    void Awake()
    {
        if( _instance != null && _instance != this ){
            Destroy(gameObject);
        }
        else{
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;


    void Start()
    {
        bgmAudioSource.volume = BgmVolume;
        sfxAudioSource.volume = SfxVolume;
    }

    #region BGM
    private float bgm_vol = -1f;
    public float BgmVolume
    {
        get{ 
            if(bgm_vol == -1)
                bgm_vol = PlayerPrefs.GetFloat("BGM_VOLUME", 1f); 
            return bgm_vol;
        }
        set{
            if( bgm_vol != value ){
                bgm_vol = value;
                PlayerPrefs.SetFloat("BGM_VOLUME", bgm_vol);

                bgmAudioSource.volume = bgm_vol;
            }
        }
    }

    public void PlayBGM()
    {
        bgmAudioSource.Play();
    }
    public void PauseBGM()
    {
        bgmAudioSource.Pause();
    }
    public void UnPauseBGM()
    {
        bgmAudioSource.UnPause();
    }
    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }
    #endregion


    #region VFX
    private float sfxVol = -1f;
    public float SfxVolume
    {
        get{
            if( sfxVol < 0 )
                sfxVol = PlayerPrefs.GetFloat( "SFX_VOLUME", 1f );
            return sfxVol;               
        }
        set{
            if( sfxVol != value ){
                sfxVol = value;
                PlayerPrefs.SetFloat("SFX_VOLUME", sfxVol);

                sfxAudioSource.volume = sfxVol;
            }
        }
    }

    public void PlaySFX(string path, float volumeScale = 1f)
    {
        PlaySFX( Resources.Load<AudioClip>(path), volumeScale );
    }
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        StopSFX();

        sfxAudioSource.PlayOneShot(clip, volumeScale);
    }
    public void StopSFX()
    {
        sfxAudioSource.Stop();
    }
    #endregion

}
