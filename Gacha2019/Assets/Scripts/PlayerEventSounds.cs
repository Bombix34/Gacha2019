using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventSounds : MonoBehaviour
{
    public void PlayFiletSound()
    {
        SoundManager.instance.PlaySound(1);
    }

    public void CatchButterflySound()
    {
        SoundManager.instance.PlaySound(2);
    }

}
