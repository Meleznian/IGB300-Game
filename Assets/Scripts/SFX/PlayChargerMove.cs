using UnityEngine;

public class PlayChargerMove : MonoBehaviour
{
   public void ChargerMove()
    {
        AudioManager.PlayEffect(SoundType.CHARGER_WALK);
    }
}
