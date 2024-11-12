using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DeviceScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        StopVideo();
    }

    public void PlayVideo()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = VideosManager.instance.GetRandomVideo();
        videoPlayer.Play();
    }
    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
    }
}
