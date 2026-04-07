using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    [SerializeField] private AudioClip[] tracks;
    [SerializeField] private bool shuffle = false;
    [SerializeField] private float musicVolume = 0.5f;

    [Header("State Music")]
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip ammoPickupClip;
    [SerializeField] private AudioClip upgradeSelectClip;
    [SerializeField] private AudioClip waveCompleteClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip hurtClip; // ✅ ADD THIS
    [SerializeField] private AudioClip buttonClickClip; // ✅ ADD THIS

    [Header("SFX Settings")]
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private int audioSourcePoolSize = 10;

    private AudioSource musicSource;
    private AudioSource[] sfxPool;
    private int poolIndex = 0;
    private int currentTrackIndex = 0;
    private bool playingStateMusic = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = false;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        sfxPool = new AudioSource[audioSourcePoolSize];
        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            sfxPool[i] = gameObject.AddComponent<AudioSource>();
            sfxPool[i].playOnAwake = false;
        }
    }

    private void Start()
    {
        if (tracks.Length > 0)
            PlayTrack(0);
    }

    private void Update()
    {
        if (!playingStateMusic && !musicSource.isPlaying && tracks.Length > 0)
            PlayNext();
    }

    // ── Background Music ──────────────────────────────────────

    public void PlayTrack(int index)
    {
        if (index < 0 || index >= tracks.Length) return;

        playingStateMusic = false;
        currentTrackIndex = index;
        musicSource.clip = tracks[currentTrackIndex];
        musicSource.loop = false;
        musicSource.Play();
    }

    public void PlayNext()
    {
        if (shuffle)
            currentTrackIndex = Random.Range(0, tracks.Length);
        else
            currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;

        PlayTrack(currentTrackIndex);
    }

    public void PlayPrevious()
    {
        currentTrackIndex = (currentTrackIndex - 1 + tracks.Length) % tracks.Length;
        PlayTrack(currentTrackIndex);
    }

    public void ResumeBGMusic()
    {
        playingStateMusic = false;
        if (tracks.Length > 0)
            PlayTrack(currentTrackIndex);
    }

    public void PauseMusic()  => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
    public void StopMusic()   => musicSource.Stop();

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        musicSource.volume = musicVolume;
    }

    public void FadeOut(float duration) => StartCoroutine(FadeCoroutine(musicSource.volume, 0f, duration));
    public void FadeIn(float duration)  => StartCoroutine(FadeCoroutine(0f, musicVolume, duration));

    private System.Collections.IEnumerator FadeCoroutine(float from, float to, float duration)
    {
        float elapsed = 0f;
        musicSource.volume = from;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        musicSource.volume = to;
    }

    // ── State Music ───────────────────────────────────────────

    public void PlayWinMusic()
    {
        if (winMusic == null) return;
        playingStateMusic = true;
        musicSource.clip = winMusic;
        musicSource.loop = false;
        musicSource.Play();
    }

    public void PlayLoseMusic()
    {
        if (loseMusic == null) return;
        playingStateMusic = true;
        musicSource.clip = loseMusic;
        musicSource.loop = false;
        musicSource.Play();
    }

    // ── SFX ───────────────────────────────────────────────────

    private void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;

        AudioSource source = sfxPool[poolIndex];
        poolIndex = (poolIndex + 1) % sfxPool.Length;

        source.clip = clip;
        source.volume = sfxVolume * volumeScale;
        source.Play();
    }

    public void PlayShoot()         => PlaySFX(shootClip);
    public void PlayAmmoPickup()    => PlaySFX(ammoPickupClip);
    public void PlayUpgradeSelect() => PlaySFX(upgradeSelectClip);
    public void PlayWaveComplete()  => PlaySFX(waveCompleteClip);
    public void PlayWin()           => PlaySFX(winClip);
    public void PlayLose()          => PlaySFX(loseClip);
    public void PlayHurt() => PlaySFX(hurtClip);
    public void PlayButtonClick() => PlaySFX(buttonClickClip);

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
    }
}