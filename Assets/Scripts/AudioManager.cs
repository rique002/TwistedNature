using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private EventInstance backgroundMusicEventInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        backgroundMusicEventInstance = CreateInstance(FMODEvents.Instance.BackgroundMusic);
        backgroundMusicEventInstance.setVolume(0.1f);
        backgroundMusicEventInstance.start();
    }

    private void Update()
    {
        if (backgroundMusicEventInstance.isValid())
        {
            backgroundMusicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 position, float volume = 1f)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        eventInstance.setVolume(volume);
        eventInstance.start();
        eventInstance.release();
    }

    public IEnumerator PlayTimedShot(EventReference sound, Vector3 position, float time)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        eventInstance.start();
        yield return new WaitForSeconds(time);
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        return eventInstance;
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        backgroundMusicEventInstance.release();
    }

    private void OnDestroy()
    {
        StopBackgroundMusic();
    }
}

