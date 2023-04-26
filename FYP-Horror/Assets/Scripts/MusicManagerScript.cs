using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource chaseMusic;
    private bool isBackgroundMusicPlaying = true;
    private bool isChaseMusicPlaying = false;

    public void playBackgroundMusic()
    {
        if (chaseMusic.isPlaying == true)
        {
            isChaseMusicPlaying = false;
        }
        backgroundMusic.Play();
        isBackgroundMusicPlaying = true;

    }

    public void playChaseMusic()
    {
        if (backgroundMusic.isPlaying == true)
        {
            isBackgroundMusicPlaying=false;
            chaseMusic.Play();
            isChaseMusicPlaying = true;
        }
    }

    public void stopAllMusic()
    {
        backgroundMusic.Stop();
        chaseMusic.Stop();
    }

    public void startFadeOut()
    {

        if (isChaseMusicPlaying == true)
        {
            StartCoroutine(fadeOutSong(chaseMusic));
            playBackgroundMusic();
        }
        else if (isBackgroundMusicPlaying == true)
        {
            StartCoroutine(fadeOutSong(backgroundMusic));
            playBackgroundMusic();
        }
    }

    IEnumerator fadeOutSong(AudioSource _music)
    {
        float baseVolumeAmount = _music.volume;
        while (_music.volume >= 0.01)
        {
            yield return new WaitForSeconds(0.1f);
            _music.volume -= 0.025f;

        }
        _music.Stop();
        _music.volume = baseVolumeAmount;
    }

}
