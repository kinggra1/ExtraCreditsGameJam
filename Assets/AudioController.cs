﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public static AudioController instance;

    public AudioClip consumeSound;

    // public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    public AudioClip backgroundAmbiance;

    public AudioClip forgeRunningSound;

    public AudioClip coinSound;
    public AudioClip woodSound;
    public AudioClip ironSound;
    public AudioClip wheatSound;
    public AudioClip questPaperSound;
    public AudioClip forgeItemSound;

    private AudioSource resourceExchangeAudioSource;
    private AudioSource musicAudioSource;
    private AudioSource backgroundAudioSource;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        resourceExchangeAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();

        musicAudioSource.clip = gameplayMusic;
        musicAudioSource.volume = 0.1f;
        musicAudioSource.loop = true;
        musicAudioSource.Play();

        backgroundAudioSource.clip = backgroundAmbiance;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void PlayPurchaseSound(int totalSale) {
        resourceExchangeAudioSource.volume = Mathf.Min(totalSale / 5f, 1f);
        resourceExchangeAudioSource.clip = coinSound;
        resourceExchangeAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        resourceExchangeAudioSource.PlayOneShot(consumeSound);
    }

    private void PlayRandomlyShiftedSound(AudioClip clip) {
        resourceExchangeAudioSource.clip = clip;
        resourceExchangeAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        resourceExchangeAudioSource.PlayOneShot(consumeSound);
    }
}