using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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
        InitializeBackgroundMusic(FMODEvents.Instance.BackgroundMusic);
    }

    private void Update()
    {
        if (backgroundMusicEventInstance.isValid())
        {
            backgroundMusicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    private void InitializeBackgroundMusic(EventReference musicEventReference)
    {
        backgroundMusicEventInstance = RuntimeManager.CreateInstance(musicEventReference);
        backgroundMusicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        backgroundMusicEventInstance.start();
        backgroundMusicEventInstance.setVolume(1);
    }

    private void OnDestroy()
    {
        backgroundMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        backgroundMusicEventInstance.release();
    }
}

