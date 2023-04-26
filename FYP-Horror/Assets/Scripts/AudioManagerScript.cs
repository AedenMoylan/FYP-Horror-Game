using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioSource jumpscareAudio;

    public void playJumpscare()
    {
        jumpscareAudio.Play();
    }
}
