using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para detectar cambios de escena

public class AudioManager : MonoBehaviour
{
    // Instancia estática para poder acceder a ella desde cualquier otro script
    // Ejemplo: AudioManager.instance.PlaySFX(...);
    public static AudioManager instance;

    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--- Audio Clip BG ---")]
    public AudioClip backgroundGame1;
    public AudioClip backgroundGame2;
    public AudioClip backgroundGame3;

    [Header("--- Audio Clip WonLost ---")]
    public AudioClip backgroundGameOver;
    public AudioClip backgroundWin;

    [Header("--- Audio Clip Protag ---")]

    public AudioClip danoioProta;

    public AudioClip dashProta;

    public AudioClip dashProtaAir;

    public AudioClip deadProta;



    [Header("--- Audio Clip Warrior ---")]

    public AudioClip warriorAtack1;

    public AudioClip warriorAtack2;

    public AudioClip warriorAtack3;



    [Header("--- Audio Clip Wizzard ---")]

    public AudioClip wizzardIgnis;

    public AudioClip wizzardFireballSound;



    [Header("--- Audio Clip Surfaces ---")]

    public AudioClip surfaceIceSlice1;

    public AudioClip surfaceIceSlice2;

    public AudioClip surfaceWalkSnow1;

    public AudioClip surfaceWalkSnow2;

    public AudioClip surfaceWalkStone1;

    public AudioClip surfaceWalkStone2;

    public AudioClip surfaceColapsedPlatform;

    void Awake()
    {
        // --- PATRÓN SINGLETON ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ¡Esto hace la magia! El objeto no se destruye.
        }
        else
        {
            // Si ya existe un AudioManager (ej. volviste al menú y cargaste uno nuevo),
            // destruye este duplicado para quedarte solo con el original.
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Iniciar música de la primera escena
        PlayMusic(backgroundGame1);
    }

    // Nos suscribimos al evento de cambio de escena
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Esta función se ejecuta automáticamente cada vez que carga una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneMusic(scene.name);
    }

    // Lógica para decidir qué música tocar según el nombre de la escena
    void CheckSceneMusic(string sceneName)
    {
        // Ejemplo de lógica (ajusta los nombres de tus escenas)
        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic(backgroundGame1);
                break;
            case "Scene Max":
                PlayMusic(backgroundGame2);
                break;
            case "Nivel3":
                PlayMusic(backgroundGame3);
                break;
            default:
                // Si no hay música específica, no hacemos nada o paramos la música
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        // Solo cambiamos la música si el clip es diferente al que ya está sonando
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    // Método helper para reproducir efectos desde otros scripts
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}