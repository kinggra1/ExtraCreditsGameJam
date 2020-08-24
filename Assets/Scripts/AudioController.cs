using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public static AudioController instance;

    // public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    public AudioClip backgroundAmbiance;

    public AudioClip forgeRunningSound;

    public AudioClip coinSound;
    public AudioClip smallCoinSound;
    public AudioClip customerArriveBell;
    public AudioClip woodSound;
    public AudioClip ironSound;
    public AudioClip wheatSound;
    public AudioClip questPaperSound;
    public AudioClip forgeHammerSound;

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

    public void PlaySoundForResource(InventorySystem.ResourceType type) {
        switch (type) {
            case InventorySystem.ResourceType.NONE:
                break;
            case InventorySystem.ResourceType.WOOD:
                PlayRandomlyShiftedSound(woodSound);
                break;
            case InventorySystem.ResourceType.IRON:
                PlayRandomlyShiftedSound(ironSound);
                break;
            case InventorySystem.ResourceType.WHEAT:
                PlayRandomlyShiftedSound(wheatSound);
                break;
        }
    }

    public void PlayForgeHammerSound() {
        PlayRandomlyShiftedSound(forgeHammerSound);
    }

    public void PlayCustomerArrivedBell() {
        resourceExchangeAudioSource.PlayOneShot(customerArriveBell);
    }

    public void PlaySmallCoinSound() {
        PlayRandomlyShiftedSound(smallCoinSound, 0.2f);
    }

    public void PlayPurchaseSound(uint totalSale) {
        resourceExchangeAudioSource.volume = Mathf.Min(totalSale / 5f, 1f);
        resourceExchangeAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        resourceExchangeAudioSource.PlayOneShot(coinSound);
    }

    private void PlayRandomlyShiftedSound(AudioClip clip) {
        PlayRandomlyShiftedSound(clip, 1f);
    }

    private void PlayRandomlyShiftedSound(AudioClip clip, float volume) {
        resourceExchangeAudioSource.volume = volume;
        resourceExchangeAudioSource.pitch = (Random.Range(0.6f, 1.1f));
        resourceExchangeAudioSource.PlayOneShot(clip);
    }
}