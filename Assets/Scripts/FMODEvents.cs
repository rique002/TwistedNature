using UnityEngine;
using FMODUnity;


public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference BackgroundMusic { get; private set; }

    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference EnemyDamage { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference PlayerDamage { get; private set; }
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }

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
