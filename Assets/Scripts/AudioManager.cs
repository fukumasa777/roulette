using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    
    public AudioClip sound1;
    public AudioClip sound2;
    public static AudioManager I { get; private set; }
    AudioSource audioSource ;
    
    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    
    public void RouletteSound()//出だしだけ
    {
        audioSource.PlayOneShot(sound1);
        StartCoroutine(DrumLoop());
    }

    IEnumerator DrumLoop()//ループ用
    {
        yield return new WaitForSeconds(0.1f);
        audioSource.Play();
    }
    

    public void ResultSound()
    {
        /*
        //ループを止める処理
        audioSource.Stop();//volumeがゼロになる可能性がある
        audioSource.volume = 1;
        */
        audioSource.PlayOneShot(sound2);
    }
    
}

