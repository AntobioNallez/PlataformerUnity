using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true; // Asegurar que la música de fondo haga loop
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(true, backgroundMusic);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetVolume);
            musicSlider.value = audioSource.volume; // Sincronizar con el volumen actual
        }
    }

    public static void SetVolume(float volume)
    {
        if (Instance != null && Instance.audioSource != null)
        {
            Instance.audioSource.volume = volume;
        }
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            Instance.audioSource.clip = audioClip;
        }

        if (Instance.audioSource.clip != null) // Asegurar que hay una canción cargada
        {
            if (resetSong) Instance.audioSource.Stop();
            Instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No hay música asignada para reproducir.");
        }
    }

    public static void PauseBackgroundMusic()
    {
        if (Instance.audioSource.isPlaying)
        {
            Instance.audioSource.Pause();
        }
    }
}
