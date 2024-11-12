using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideosManager : MonoBehaviour
{
    public static VideosManager instance {  get; private set; }
    [SerializeField] private VideosClipRefsSO videosClipRefsSO;
    private void Awake()
    {
        instance = this; 
    }
    public VideoClip GetRandomVideo()
    {
        return videosClipRefsSO.videoClips[Random.Range(0, videosClipRefsSO.videoClips.Length)];
    }
}
