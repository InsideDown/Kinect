using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AudioController : MonoBehaviour
{
    [Serializable]
    public struct KaraokeLine
    {
        public float Time;
        public string Lyric;
    }
    public TextMeshProUGUI Text;
    public GameObject TextContainer;
    private BackgroundRemovalManager _BackgroundRemover;
    public List<KaraokeLine> KaraokeList = new List<KaraokeLine>();

    private AudioSource _SongAudioSource;
    private int _AudioIndex = -1;
    private KinectManager _KinectManager;

    private void Awake()
    {
        _SongAudioSource = this.gameObject.GetComponent<AudioSource>();

        if (_SongAudioSource == null)
            throw new Exception("A SongAudioSource must be defined in AudioController");

        if (TextContainer == null)
            throw new Exception("A TextContainer must be defined in AudioController");

        TextContainer.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        _KinectManager = KinectManager.Instance;
        _BackgroundRemover = BackgroundRemovalManager.Instance;
    }

    private void SetLyric(string lyricStr)
    {
        TextContainer.SetActive(true);
        Text.text = lyricStr;
    }

    // Update is called once per frame
    void Update()
    {
        //only play our audio if we have a user
        if(_KinectManager)
        {
            if (_KinectManager.GetUsersCount() > 0)
            {
                if (!_SongAudioSource.isPlaying)
                    _SongAudioSource.Play();
            }else
            {
                _SongAudioSource.Pause();
            }
        }

        float curTime = _SongAudioSource.time;

        /*if(curTime > 10)
        {
            if(_BackgroundRemover.invertAlphaColorMask)
            {
                Debug.Log("inverting mask");
                _BackgroundRemover.InvertMask(false);
            }
        }*/

        //find the time that we're greater than but less than and set our index
        for(int i=0;i<KaraokeList.Count;i++)
        {
            float indexTime = KaraokeList[i].Time;
            string lyric = KaraokeList[i].Lyric;
            if (indexTime < curTime)
            {
                //we've passed the current time, however we need to make sure we're less than the next line
                if(i + 1 < KaraokeList.Count)
                {
                    float nextTime = KaraokeList[(i + 1)].Time;
                    if(nextTime > curTime)
                    {
                        _AudioIndex = i;
                        SetLyric(lyric);
                        break;
                    }
                }
            }
        }
    }
}
