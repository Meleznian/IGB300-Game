using UnityEngine;

public class PlaySteamKing : MonoBehaviour
{
    public void playThrust()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_THRUST,1.5f);
    }
    public void playDash()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_DASH);
    }
    public void playJump()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_JUMP);
    }
    public void playLand()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_LAND, 1.5f);
    }
    public void playChain()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_CHAIN);
    }
    public void playDie1()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_DIE_1);
    }
    public void playDie2()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_DIE_2);
    }
    public void playShoot()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_SHOOT);
    }

    public void playLaugh()
    {
        AudioManager.PlayEffect(SoundType.STEAM_KING_LAUGH);
    }
}
