using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallSounds : MonoBehaviour {
    [SerializeField] private AudioClip[] bounceSounds;
    [SerializeField] private AudioClip[] netSounds;
    [SerializeField] private AudioClip[] ringSounds;
    [SerializeField] private AudioClip[] shieldSounds;
    [SerializeField] private AudioClip[] throwSounds;

    private static AudioSource source;
    private static Dictionary<SoundType, AudioClip[]> soundDict = new Dictionary<SoundType, AudioClip[]>();

    private void Awake() {
        source = GetComponent<AudioSource>();
        soundDict.Add(SoundType.Bounce, bounceSounds);
        soundDict.Add(SoundType.Net, netSounds);
        soundDict.Add(SoundType.Ring, ringSounds);
        soundDict.Add(SoundType.Shield, shieldSounds);
        soundDict.Add(SoundType.Throw, throwSounds);
    }

    public static bool SwitchVolume() {
        source.mute = !source.mute;
        return !source.mute;
    }

    public static void PlaySound(SoundType soundType) {
        PlayRandom(soundDict[soundType]);
    }

    private static void PlayRandom(AudioClip[] clips) {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = 1 + Random.Range(-0.05f, 0.05f);
        source.PlayOneShot(clip);
    }
}

public enum SoundType {
    Bounce,
    Net,
    Ring,
    Shield,
    Throw
}