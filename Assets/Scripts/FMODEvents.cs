using UnityEngine;
using FMODUnity;


public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference BackgroundMusic { get; private set; }
    [field: SerializeField] public EventReference GameOverMusic { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference EnemyDamage { get; private set; }

    [field: Header("Bird SFX")]
    [field: SerializeField] public EventReference BirdDamage { get; private set; }
    [field: SerializeField] public EventReference BirdFootsteps { get; private set; }
    [field: SerializeField] public EventReference BirdHit { get; private set; }

    [field: Header("Hedgehog SFX")]
    [field: SerializeField] public EventReference HedgehogDamage { get; private set; }
    [field: SerializeField] public EventReference HedgehogFootsteps { get; private set; }
    [field: SerializeField] public EventReference HedgehogHit { get; private set; }
    [field: SerializeField] public EventReference HedhehogVoice { get; private set; }

    [field: Header("General SFX")]
    [field: SerializeField] public EventReference SquareChange { get; private set; }
    [field: SerializeField] public EventReference DoorOpening { get; private set; }

    public static FMODEvents Instance { get; private set; }

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
}
