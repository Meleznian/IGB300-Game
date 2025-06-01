using UnityEngine;

public class PlayTurretAim : MonoBehaviour
{
    public void PlayAim()
    {
        AudioManager.PlayEffect(SoundType.TURRET_LOCKON);

    }
}
