using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PrepareVideos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer[] videos = GameObject.FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer video in videos)
        {
            StartCoroutine(prepareVideo(video));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator<int> prepareVideo(VideoPlayer video)
    {
        video.Prepare();

        //Wait until video is prepared
        while (!video.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return 1;
        }
    }
}
