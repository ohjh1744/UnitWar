using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource _bgmSource;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _uiSource;

    private Dictionary<string, AudioClip> audioClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
    }

    public void PlayBGM(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            _bgmSource.clip = audioClips[clipName];
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM clip '{clipName}' not found!");
        }
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    public void PlaySFX(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            _sfxSource.PlayOneShot(audioClips[clipName]);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }

    public void PlayUI(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            _uiSource.PlayOneShot(audioClips[clipName]);
        }
        else
        {
            Debug.LogWarning($"UI clip '{clipName}' not found!");
        }
    }
}
