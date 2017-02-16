using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectSource : MonoBehaviour {
    public static SoundEffectSource instance;
    public AudioSource AudioSource { get; private set; }

    void Awake() {
        if (instance)
            Destroy(this);
        else {
            instance = this;
            AudioSource = GetComponent<AudioSource>();
        }
    }
}
