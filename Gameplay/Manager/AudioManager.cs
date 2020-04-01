using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    private AudioSource audioSource;
    private AudioSource audioSource2;
    private static Dictionary<string, AudioClip> loaded = new Dictionary<string, AudioClip>();
    private AudioClip bossTheme;
    private AudioClip haloTheme;
    private float secondThemeStart;
    private float secondThemeEnd;
    private bool playedIntro = false;
    private float defaultSfxVolume = 0.3f;
    private int musicTime = 0;
    private bool playMusic = true;
    // Special case, could be done better but release in less than a month and this works.
    private int finalSongStep = 0;
    private const float finalSongMidBreakPoint = 88;

    public static AudioManager GetInstance() {
        if (instance == null) {
            instance = new GameObject().AddComponent<AudioManager>();
            instance.name = "Audio Manager";
        }
        return instance;
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(instance);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }

    private void Update() {
        if (audioSource2.isPlaying && secondThemeEnd <= Time.time) {
            StopSecondaryMusic();
        }

        if (playMusic && !audioSource.isPlaying && !audioSource2.isPlaying) {
            PlaySceneTheme(false);
        }

        // Manage loops on songs with intros.
        var scene = SceneManager.GetActiveScene().name;
        if (playMusic && scene == Scenes.FINAL_ZONE && audioSource.isPlaying) {
            if (finalSongStep == 0 && audioSource.time >= 41) {
                audioSource.time = 0;
            }
            if (finalSongStep == 1 && audioSource.time >= finalSongMidBreakPoint) {
                finalSongStep = 2;
            }
            if (finalSongStep == 2 && (audioSource.time >= 159.99f || audioSource.time < finalSongMidBreakPoint)) {
                audioSource.time = finalSongMidBreakPoint;
            }
        }

        // TODO: Fix
        // Special case for cemetery
        if (playMusic && scene == Scenes.MONASTERY_ACT_2 && audioSource.isPlaying && audioSource.time >= 73.5) {
            audioSource.time = 0.33f;
        }
    }

    // Chapuza php tier
    public void SetFinalBossSongPastIntro() {
        finalSongStep = 1;
        if (audioSource.time < 41) {
            audioSource.time = 41;
        }
    }

    public void SetFinalBossSongOnLoop() {
        finalSongStep = 2;
    }

    public void ResetFinalBossSong() {
        finalSongStep = 0;
    }

    public void FinalBossSongFinished() {
        finalSongStep = 3;
    }

    public void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.U)) {
            audioSource.time = 155;
        }
    }

    // We need some effects to be preloaded.
    public void LoadSounds() {
        LoadSfx(Sfx.JUMP);
        LoadSfx(Sfx.BOSS_HIT);
        LoadSfx(Sfx.PICK_WAFER);
        LoadSfx(Sfx.POWER_UP);
        LoadSfx(Sfx.PICK_ITEM);
        LoadSfx(Sfx.BABY_PICKUP);
        LoadSfx(Sfx.CHECKPOINT);
        LoadSfx(Sfx.ENEMY_HIT);
        LoadMusic(Music.BOSS);
        LoadMusic(Music.INVULNERABLE);
    }

    // TODO: Refactor
    public void LoadSfx(string name) {
        LoadSound("Sfx", name);
    }

    public void LoadMusic(string name) {
        LoadSound("Music", name);
    }

    private void LoadSound(string type, string name) {
        if (!loaded.ContainsKey(name)) {
            loaded.Add(name, Resources.Load<AudioClip>("Audio/" + type + "/" + name));
        }
    }

    public void PlaySceneTheme(bool resetTime) {
        if (resetTime) {
            musicTime = 0;
            playedIntro = false;
        }
        audioSource.timeSamples = musicTime;

        // TODO: I mean it's pretty obvious what I have to do here.
        // I wish I had good pattern matching in C#
        var name = SceneManager.GetActiveScene().name;
        if (name == Scenes.INTRO) {
            PlayMusic(Music.INTRO_TEXT);
        }
        if (name == Scenes.MAIN_MENU) {
            PlayMusic(Music.MAIN_MENU);
        }
        if (name == Scenes.MONASTERY_ACT_1) {
            PlayMusic(Music.MONASTERY_ACT_1);
        }
        if (name == Scenes.MONASTERY_ACT_2) {
            PlayMusic(Music.MONASTERY_ACT_2);
        }
        if (name == Scenes.NIGHT_BAR_ACT_1 || name == Scenes.NIGHT_BAR_ACT_2) {
            if (!playedIntro) {
                playedIntro = true;
                PlayMusicOnce(Music.NIGHT_BAR_INTRO);
            } else {
                PlayMusic(Music.NIGHT_BAR_ACT_1);
            }
        }
        if (name == Scenes.HOSPITAL_ACT_1 || name == Scenes.HOSPITAL_ACT_2) {
            PlayMusic(Music.HOSPITAL_ACT_1);
        }
        if (name == Scenes.DESERT_ACT_1 || name == Scenes.DESERT_ACT_2) {
            PlayMusic(Music.DESERT_ACT_1);
        }
        if (name == Scenes.LAB_ACT_1 || name == Scenes.LAB_ACT_2) {
            PlayMusic(Music.LAB_ACT_1);
        }
        if (name == Scenes.COMMIE_HQ_ACT_1 || name == Scenes.COMMIE_HQ_ACT_2) {
            PlayMusic(Music.COMMIE_HQ_ACT_1);
        }
        if (name == Scenes.FINAL_ZONE) {
            PlayMusic(Music.FINAL_ZONE);
        }
    }

    private AudioClip GetMusicClip(string name) {
        return GetClip("Music", name);
    }

    private AudioClip GetSfxClip(string name) {
        return GetClip("Sfx", name);
    }

    private AudioClip GetClip(string type, string name) {
        if (!loaded.ContainsKey(name)) {
            var resource = Resources.Load<AudioClip>("Audio/" + type + "/" + name);
            if (resource != null) {
                loaded.Add(name, Resources.Load<AudioClip>("Audio/" + type + "/" + name));
            }
        }

        return loaded[name];
    }

    public void UnloadClip(string name) {
        if (loaded.ContainsKey(name)) {
            Resources.UnloadAsset(loaded[name]);
            loaded.Remove(name);
        }
    }

    public void PlayMusic(string music) {
        PlayMusic(music, 0f, true);
    }

    public void PlayMusicOnce(string music) {
        PlayMusic(music, 0f, false);
    }

    private void PlayMusic(string music, float start, bool loop) {
        playMusic = false;
        audioSource.Stop();
        audioSource2.Stop();
        var clip = GetMusicClip(music);
        if (clip != null) {
            audioSource.volume = Mathf.Clamp(1 * GameState.musicVolume / 10, 0f, 1f);
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.loop = loop;
            audioSource.time = start;
            playMusic = true;
        }
    }

    public void PauseMusic() {
        playMusic = false;
        audioSource.Pause();
        audioSource2.Pause();
    }

    public void UnpauseMusic() {
        playMusic = true;
        audioSource.UnPause();
        audioSource2.UnPause();
    }

    public void PlayEffect(string effect) {
        PlayEffect(effect, defaultSfxVolume);
    }

    public void PlayEffect(string effect, float volume) {
        volume = Mathf.Clamp(volume * GameState.sfxVolume / 10, 0f, 1f);
        audioSource.PlayOneShot(GetSfxClip(effect), volume);
    }

    /*
    public IEnumerator<WaitForSeconds> PlayPiano() {
        float scale = Mathf.Pow(2f, 1.0f / 12f);
        float volume = Mathf.Clamp(1 * GameState.sfxVolume / 10, 0f, 1f);
        for (int i = 0; i < 72; i++) {
            
            audioSource.pitch = Mathf.Pow(scale, i);
            audioSource.PlayOneShot(GetSfxClip(Sfx.PIANO), volume);
            yield return new WaitForSeconds(0.2f);
        }
    }*/

    public void PlaySecondaryMusic(string music, float time) {
        playMusic = true;
        musicTime = audioSource.timeSamples;
        audioSource.Stop();
        audioSource2.volume = Mathf.Clamp(1 * GameState.musicVolume / 10, 0f, 1f);
        audioSource2.clip = GetMusicClip(music);
        audioSource2.Play();
        audioSource2.loop = true;
        secondThemeStart = Time.time;
        secondThemeEnd = secondThemeStart + time;
    }

    public void StopAllMusic() {
        playMusic = false;
        audioSource.Stop();
        audioSource.timeSamples = 0;
        audioSource2.Stop();
        audioSource2.timeSamples = 0;
    }

    public void StopSecondaryMusic() {
        audioSource2.Stop();
        audioSource.Play();
    }

    public void FasterSong() {
        audioSource.pitch *= 1.3f;
    }

    public void UndoFasterSong() {
        audioSource.pitch = 1f;
    }
}
