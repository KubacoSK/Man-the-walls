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
        // zacne hrat nahodnu skladbu
        currentTrackIndex = Random.Range(0, backgroundMusic.Length);
        backgroundMusic[currentTrackIndex].Play();
       
    }

    private void Update()
    {
        if (!backgroundMusic[currentTrackIndex].isPlaying && !PauseMenu.instance.IsSettingsMenuPlayingSound())
        {
            PlayNextTrack();
        }
    }
    public void PlayNextTrack()
    {
        // zastavi vsetky skladby
        foreach (var music in backgroundMusic)
        {
            music.Stop();
        }

        int randomTrackIndex;
        do    // vybere nahodny song
        {
            randomTrackIndex = UnityEngine.Random.Range(0, backgroundMusic.Length);
        } while (randomTrackIndex == currentTrackIndex);


        backgroundMusic[randomTrackIndex].Play();


        currentTrackIndex = randomTrackIndex;
    }


    public void PauseBackgroundMusic()
    {
        if (backgroundMusic[currentTrackIndex].isPlaying)
            backgroundMusic[currentTrackIndex].Pause();
    }
    public void ResumeBackgroundMusic()
    {
        if (!backgroundMusic[currentTrackIndex].isPlaying)
        backgroundMusic[currentTrackIndex].UnPause();
    }
}