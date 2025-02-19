using Unity.VisualScripting;
using UnityEngine;

public class CombatSound : MonoBehaviour
{
    [SerializeField] private AudioSource combatSound;
    public static CombatSound Instance;

    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one CombatSound! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayCombatSounds()
    {
        if (!combatSound.isPlaying)
            combatSound.Play();
    }

    public void StopCombatSounds()
    {
        if (combatSound.isPlaying)
            combatSound.Stop();
    }

}
