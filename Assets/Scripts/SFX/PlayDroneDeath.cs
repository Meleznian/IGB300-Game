using UnityEngine;

public class PlayDroneDeath : MonoBehaviour
{
    public void playSound()
    {
        AudioManager.PlayEffect(SoundType.KAMIKAZE_EXPLODE);
    }
}
