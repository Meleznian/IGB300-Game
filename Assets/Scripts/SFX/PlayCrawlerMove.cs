using UnityEngine;
using System;
using System.Collections;

public class PlayCrawlerMove : MonoBehaviour
{
    public void PlayMove()
    {
        AudioManager.PlayEffect(SoundType.CRAWLER_WALK, 0.1f);
    }
}
