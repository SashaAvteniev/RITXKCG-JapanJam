using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    // BGMのキーをここに追加する
    Title,
    MainGame,
    Test,
    
    None
}

public enum SFX
{
    // SEのキーをここに追加する
    Punch,
    ButtonPress,
    Fall,
    Jump,
    MoveForward,

    None
}

[System.Serializable]
public class BGMPair
{
    public BGM _bgmKey;
    public AudioClip _clip;
}

[System.Serializable]
public class SFXPair
{
    public SFX _sfxKey;
    public AudioClip _clip;
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private static float _bgmVolume = 0.5f;

    [SerializeField, Range(0f, 1f)]
    private static float _sfxVolume = 0.5f;

    public float BgmVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = Mathf.Clamp01(value);
            if (_bgmSource != null)
                _bgmSource.volume = _bgmVolume;
        }
    }

    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = Mathf.Clamp01(value);
            if (_sfxSource != null)
                _sfxSource.volume = _sfxVolume;
        }
    }


    [SerializeField]
    private BGMPair[] _bgm;

    [SerializeField]
    private SFXPair[] _sfx;

    private Dictionary<BGM, AudioClip> _bgmTable;
    private Dictionary<SFX, AudioClip> _sfxTable;

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    public void Initialize()
    {
        BuildTables();
        CreateAudioSources();
    }

    private void BuildTables()
    {
        _bgmTable = new Dictionary<BGM, AudioClip>();
        foreach (var pair in _bgm)
        {
            if (pair._bgmKey == BGM.None || pair._clip == null)
                continue;

            _bgmTable[pair._bgmKey] = pair._clip;
        }

        _sfxTable = new Dictionary<SFX, AudioClip>();
        foreach (var pair in _sfx)
        {
            if (pair._sfxKey == SFX.None || pair._clip == null)
                continue;

            _sfxTable[pair._sfxKey] = pair._clip;
        }
    }

    private void CreateAudioSources()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;
        _bgmSource.volume = _bgmVolume;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;
        _sfxSource.volume = _sfxVolume;
    }

    // =====================
    // BGM
    // =====================
    public void PlayBGM(BGM key)
    {
        if (!_bgmTable.TryGetValue(key, out var clip))
            return;

        if (_bgmSource.clip == clip && _bgmSource.isPlaying)
            return;

        _bgmSource.Stop();
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    // =====================
    // SFX
    // =====================
    public void PlaySE(SFX key, AudioSource source = null)
    {
        /*
        if (!_sfxTable.TryGetValue(key, out var clip))
            return;

        if (source == null)
            _sfxSource.PlayOneShot(clip);
        else
            source.PlayOneShot(clip);
        */
    }
}