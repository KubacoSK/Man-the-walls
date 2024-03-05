using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource[] backgroundMusic;
    private int currentTrackIndex;
    public static MusicPlayer Instance;

    private void Start()
    {
        // Start playing the first background music track
        backgroundMusic[0].Play();
        currentTrackIndex = 0;
    }

    private void Update()
    {
        if (!backgroundMusic[currentTrackIndex].isPlaying)
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
        do
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
        backgroundMusic[currentTrackIndex].Pause();
    }
    public void ResumeBackgroundMusic()
    {
        backgroundMusic[currentTrackIndex].UnPause();
    }
}