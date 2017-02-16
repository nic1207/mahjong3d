using UnityEngine;
using System.Collections;

public class SoundEffect : MonoBehaviour {
    private bool _isFadeOut = false;
    private float _volume;

    public void PlayLoop(AudioClip clip) {
        SoundEffectSource.instance.AudioSource.clip = clip;
        SoundEffectSource.instance.AudioSource.loop = true;
        SoundEffectSource.instance.AudioSource.Play();
    }

    public void Stop() {
        SoundEffectSource.instance.AudioSource.Stop();
    }

    public void PlayOnce(AudioClip clip) {
        if (SoundEffectSource.instance)
            SoundEffectSource.instance.AudioSource.PlayOneShot(clip);
    }

    public void FadeOut() {
        if (SoundEffectSource.instance) {
            _volume = SoundEffectSource.instance.AudioSource.volume;
            _isFadeOut = true;
        }
    }

    void Update() {
        if (_isFadeOut && SoundEffectSource.instance) {
            if (SoundEffectSource.instance.AudioSource.volume > 0)
            {
                SoundEffectSource.instance.AudioSource.volume -= 0.05f * Time.deltaTime;
            } else if (SoundEffectSource.instance.AudioSource.volume < 0.01f) {
                Stop();
                SoundEffectSource.instance.AudioSource.volume = _volume;
                _isFadeOut = false;
            }
        }
    }
}
