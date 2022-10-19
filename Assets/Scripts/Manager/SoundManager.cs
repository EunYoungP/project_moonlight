using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND
{
    Bgm,
    Effect,
    Step,
}

public class SoundManager : MonoBehaviour
{
    private AudioSource[] audioSourceArr = new AudioSource[System.Enum.GetValues(typeof(SOUND)).Length];
    private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject soundObject = new GameObject("SoundManager",typeof(SoundManager));
                DontDestroyOnLoad(soundObject);
                instance = soundObject.GetComponent<SoundManager>();
                instance.Init();
            }
            return instance;
        }
    }

    public void Init()
    {
        GameObject soundRoot = GameObject.Find("SoundManager");

        string[] Sources = System.Enum.GetNames(typeof(SOUND));
        for(int i = 0; i<Sources.Length; i++)
        {
            GameObject soundSourceObject = new GameObject(Sources[i]);
            audioSourceArr[i] = soundSourceObject.AddComponent<AudioSource>();
            soundSourceObject.transform.parent = soundRoot.transform;
        }
        audioSourceArr[(int)SOUND.Bgm].loop = true;
        audioSourceArr[(int)SOUND.Step].loop = true;
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in audioSourceArr)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        audioClipDic.Clear();
    }

    public void StopAudioSource(SOUND type)
    {
        audioSourceArr[(int)type].Stop();
    }

    // 오디오소스에 클립, 피치을 채워넣어주고 플레이 시켜줍니다.
    private void PlaySound(AudioClip audioClip, SOUND type = SOUND.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if(type == SOUND.Bgm)
        {
            AudioSource audioSource = audioSourceArr[(int)SOUND.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
        else if(type == SOUND.Effect)
        {
            AudioSource audioSource = audioSourceArr[(int)SOUND.Effect];

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else if (type == SOUND.Step)
        {
            AudioSource audioSource = audioSourceArr[(int)SOUND.Step];

            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
    }

    public void Play(string path, SOUND type = SOUND.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        PlaySound(audioClip, type, pitch);
    }

    // path 와 type 을 받아 Sound파일로부터 audioClip을 받아옵니다.
    // Resoucre.load 프로퍼티 구조 분석
    public AudioClip GetOrAddAudioClip(string path, SOUND type = SOUND.Effect)
    {
        AudioClip audioClip = null;

        if (path.Contains("Prefabs/Sound/") == false)
        {
            path = $"Prefabs/Sound/{path}";
        }

        if (type == SOUND.Bgm)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else if(type == SOUND.Effect)
        {
            if (audioClipDic.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                audioClipDic.Add(path, audioClip);
            }
            else
                audioClip = audioClipDic[path];
        }
        else if(type == SOUND.Step)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }

        if (audioClip == null)
            Debug.Log("해당하는 오디오 클립 없음");

        return audioClip;
    }
}
