﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameObject : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void VolumeUpdate()
    {
        audioSource.volume = ApplicationUtil.GameVolume;
    }

    public void MusicUpdate()
    {
        if (ApplicationUtil.GameMusic)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
