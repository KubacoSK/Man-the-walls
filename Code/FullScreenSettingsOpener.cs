using UnityEngine;

public class FullScreenSettingsOpener : MonoBehaviour
{
    public void OpenFullScreenSettings()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))    // otvori android settings na nastavenie displeja
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.DISPLAY_SETTINGS"))
            {
                activity.Call("startActivity", intent);
            }
        }
    }
}