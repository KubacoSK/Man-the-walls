using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource[] backgroundMusic;
    private int currentTrackIndex;
    public static MusicPlayer Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one MusicPlayer! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        // Start playing the first background music track
        backgroundMusic[0].Play();
        currentTrackIndex = 0;
    }

    private void Update()
    {
        if (!backgroundMusic[currentTrackIndex].isPlaying && !PauseMenu.instance.IsSettingsMenuPlayingSound())
        {
            // Play the next track
            PlayNextTrack();
        }
    }
    public void PlayNextTrack()
    {
        // Stop all currently playing music tracks
        foreach (var music in backgroundMusic)
        {
            music.Stop();
        }

        // Generate a random track index
        int randomTrackIndex;
        do    // chooses random track that isnt last one
        {
            randomTrackIndex = UnityEngine.Random.Range(0, backgroundMusic.Length);
        } while (randomTrackIndex == currentTrackIndex);

        // Play the randomly selected track
        backgroundMusic[randomTrackIndex].Play();

        // Update the current track index
        currentTrackIndex = randomTrackIndex;
    }

    // Call this method to stop the background music
    public void PauseBackgroundMusic()
    {
        if (backgroundMusic[currentTrackIndex].isPlaying)
            backgroundMusic[currentTrackIndex].Pause();
    }
    // This one resumes it after it has been stopped
    public void ResumeBackgroundMusic()
    {
        if (!backgroundMusic[currentTrackIndex].isPlaying)
        backgroundMusic[currentTrackIndex].UnPause();
    }
}