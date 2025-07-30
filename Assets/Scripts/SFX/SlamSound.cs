using UnityEngine;

public class SlamSound : MonoBehaviour
{
    public AudioSource slamAudio;

    public void PlaySlamSFX()
    {
        if (slamAudio != null)
        {
            slamAudio.Play();
        }
    }
}
