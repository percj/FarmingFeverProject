using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepsController : MonoBehaviour
{
    AudioClip currAudio;
    //Sprite footPrint;
    public void step()
    {
        if(currAudio != null) GameSingleton.Instance.Sounds.audioSource.PlayOneShot(currAudio,0.5f);
       // if (footPrint != null) GameSingleton.Instantiate(footPrint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ground ground))
        {
            currAudio = ground.clip;
           // footPrint = ground.footPrints;
        }

    }
}
