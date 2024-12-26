using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public HapticSource hapticSource;
    public AudioSource audioSource;
    public AudioClip Drop;
    public AudioClip Take;
    public AudioClip OpenStation;
    public AudioClip CashCollect;
    public AudioClip ButtonClick;
    public AudioClip OpenMarket;
    public AudioClip TrashDrop;
  

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip); HapticFeedback();
    }
    public void ButtonOneShot()
    {
        audioSource.PlayOneShot(ButtonClick); HapticFeedback();
    }
    public void HapticFeedback()
    {
        hapticSource.Play();
    }
    public void HapticClick()
    {
        hapticSource.level = 0;
        AudioListener.volume = 1;
    }
    public void SoundClick()
    {
        AudioListener.volume = 0;
        hapticSource.level = 1;
    }
    
}
