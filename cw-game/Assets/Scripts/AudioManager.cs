using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("--- Audio Source ---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--- Audio Clip BG ---")]
    public AudioClip backgroundGame1;
    public AudioClip backgroundGame2;
    public AudioClip backgroundGame3;
    public AudioClip backgroundGame4; // Agregado según tu lista
    
    [Header("--- Audio Boss Batle ---")]

    public AudioClip backgroundBossBattle;

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

    [Header("-- Audio Clip Pin Distance ---")]
    public AudioClip danioDista;
    public AudioClip distaAtack;
    public AudioClip distanceExisting;

    [Header("-- Audio Clip Pin Melee --")]
    public AudioClip danioMelee;
    public AudioClip meleeAtack;
    public AudioClip meleeAtack2;
    public AudioClip meleeDash;
    public AudioClip meleeExisting;
    public AudioClip muerteMelee;

    // Referencia para controlar y detener la lista de reproducción
    private Coroutine currentMusicCoroutine;

    void Awake()
    {
        Debug.Log($"[AudioManager] Naciendo... ID: {gameObject.GetInstanceID()} en Escena: {SceneManager.GetActiveScene().name}");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] ¡Detectado duplicado! Destruyendo el nuevo que intentó nacer en: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        // Si el objeto se destruye, nos avisará
        Debug.LogWarning($"[AudioManager] ¡ME ESTÁN DESTRUYENDO! ID: {gameObject.GetInstanceID()}. Causa probable: Carga de escena o destrucción manual.");
    }

    void Start()
    {
        PlayMusic(backgroundGame1);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneMusic(scene.name);
    }

    void CheckSceneMusic(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic(backgroundGame1, true);
                break;

            case "Tower":
                StartTowerPlaylist();
                break;

            case "Nivel3":
                PlayMusic(backgroundGame3, true);
                break;

            case "BossBattle":
                PlayMusic(backgroundBossBattle, true);
                break;

            default:
                break;
        }
    }

    // --- MÉTODOS DE MÚSICA Y SFX GENÉRICOS ---



    public void PlayMusic(AudioClip clip, bool shouldLoop = true)
    {
        StopMusicCoroutine();

        if (musicSource.clip != clip)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.loop = shouldLoop;
            musicSource.Play();
        }
        else
        {
            if (musicSource.loop != shouldLoop)
                musicSource.loop = shouldLoop;

            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            // Variamos ligeramente el tono (pitch) para que no suene robótico
            // Esto es opcional, pero hace que los juegos se sientan más profesionales
            SFXSource.pitch = Random.Range(0.9f, 1.1f);
            SFXSource.PlayOneShot(clip);
            SFXSource.pitch = 1f; // Reseteamos el pitch
        }
    }

    // --- CORRUTINA DE PLAYLIST ---
    public void StartTowerPlaylist()
    {
        StopMusicCoroutine();
        currentMusicCoroutine = StartCoroutine(PlayPlaylistRoutine());
    }

    private void StopMusicCoroutine()
    {
        if (currentMusicCoroutine != null)
        {
            StopCoroutine(currentMusicCoroutine);
            currentMusicCoroutine = null;
        }
    }

    IEnumerator PlayPlaylistRoutine()
    {
        AudioClip[] playlist = { backgroundGame2, backgroundGame3, backgroundGame4 };
        int currentIndex = 0;

        while (true)
        {
            AudioClip currentClip = playlist[currentIndex];

            if (currentClip != null)
            {
                musicSource.clip = currentClip;
                musicSource.loop = false;

                for (int i = 0; i < 2; i++)
                {
                    musicSource.Play();
                    yield return new WaitForSeconds(currentClip.length);
                }

                musicSource.Stop();
                yield return new WaitForSeconds(10f); // Silencio de 10s
            }
            else
            {
                Debug.LogWarning($"Clip nulo en playlist index {currentIndex}");
                yield return null;
            }

            currentIndex++;
            if (currentIndex >= playlist.Length) currentIndex = 0;
        }
    }

    // ========================================================================
    // --- NUEVA SECCIÓN: MÉTODOS PÚBLICOS PARA LLAMAR DESDE OTROS SCRIPTS ---
    // ========================================================================

    // PROTAGONISTA
    public void PlayProtaDamage() => PlaySFX(danoioProta);
    public void PlayProtaDash() => PlaySFX(dashProta);
    public void PlayProtaDashAir() => PlaySFX(dashProtaAir);
    public void PlayProtaDead() => PlaySFX(deadProta);

    //Pinguino Distancia
    public void PlayPinDisDanio() => PlaySFX(danioDista);
    public void PlayPinDisAtack() => PlaySFX(distaAtack);

    public void PlayPinDisExisting() => PlaySFX(distanceExisting);

    //Pinguino Melee
    public void PlayPinMeleeDanio() => PlaySFX(danioMelee);
    public void PlayPinMeleeAtack() => PlaySFX(meleeAtack);
    public void PlayPinMeleeDash() => PlaySFX(meleeDash);
    public void PlayPinMeleeExisting() => PlaySFX(meleeExisting);
    public void PlayPinMeleeMuerte() => PlaySFX(muerteMelee);


    // GUERRERO (WARRIOR) - Lógica aleatoria incluida
    public void PlayWarriorAttack()
    {
        // Elige uno de los 3 ataques al azar para dar variedad
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0: PlaySFX(warriorAtack1); break;
            case 1: PlaySFX(warriorAtack2); break;
            case 2: PlaySFX(warriorAtack3); break;
        }
    }

    // MAGO (WIZZARD)
    public void PlayWizzardIgnis() => PlaySFX(wizzardIgnis);
    public void PlayWizzardFireball() => PlaySFX(wizzardFireballSound);

    // SUPERFICIES (Pasos)
    // Puedes llamar esto desde el script de movimiento cuando detecte suelo
    public void PlayFootstepSnow()
    {
        // Alterna aleatoriamente entre los dos sonidos de nieve
        if (Random.value > 0.5f) PlaySFX(surfaceWalkSnow1);
        else PlaySFX(surfaceWalkSnow2);
    }

    public void PlayFootstepStone()
    {
        if (Random.value > 0.5f) PlaySFX(surfaceWalkStone1);
        else PlaySFX(surfaceWalkStone2);
    }

    public void PlayFootstepIce()
    {
        if (Random.value > 0.5f) PlaySFX(surfaceIceSlice1);
        else PlaySFX(surfaceIceSlice2);
    }

    public void PlayFootstepColapsed()
    {
        if (Random.value > 0.5f) PlaySFX(surfaceColapsedPlatform);
        else PlaySFX(surfaceColapsedPlatform);
    }


}