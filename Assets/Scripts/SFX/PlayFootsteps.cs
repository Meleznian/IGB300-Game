using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayFootsteps : MonoBehaviour
{
    public void PlaySound()
    {
        AudioManager.PlayEffect(SoundType.WALK);
    }
}
