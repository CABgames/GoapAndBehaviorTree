/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: AudioManager.cs 
///Created by: Charlie Bullock
///Description: Manages the small amount of audio played at when an ai is detected or a score gained
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Variables
    [SerializeField]
    private AudioClip guardScoreSound;
    [SerializeField]
    private AudioClip spyScoreSound;
    [SerializeField]
    private AudioClip guardAlertSound;
    [SerializeField]
    private AudioClip spyAlertSound;
    [SerializeField]
    private AudioSource source;

    public void AlertSound(bool isGuard)
    {
        if (isGuard)
        {
            source.PlayOneShot(guardAlertSound,1);
        }
        else
        {
            source.PlayOneShot(spyAlertSound,1);
        }
    }

    public void ScoreSound(bool isGuard)
    {
        if (isGuard)
        {
            source.PlayOneShot(guardScoreSound,1);
        }
        else
        {
            source.PlayOneShot(spyScoreSound,1);
        }
    }
}
