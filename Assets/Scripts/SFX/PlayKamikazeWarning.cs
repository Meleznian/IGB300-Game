using UnityEngine;

public class PlayKamikazeWarning : MonoBehaviour
{
    public void playSound()
    {
        AudioManager.PlayEffect(SoundType.KAMIKAZE_WARNING);
    }
}
