using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerGenerator : MonoBehaviour
{
    public LevelChunk[] chunkPrefabs; // Array para poner tus moldes de secciones
    public int totalChunks = 5;       // Cuántas secciones apilar
    public GameObject goalPrefab;     // La meta final

    private float currentHeight = 0f; // Puntero de altura

    void Start()
    {
        GenerateTower();
    }

    void GenerateTower()
    {
        // Punto de inicio (donde pusiste el Generador)
        currentHeight = transform.position.y;

        for (int i = 0; i < totalChunks; i++)
        {
            // Si llegamos al piso 10 (índice 9) cargamos la escena del jefe
            if (i >= 9)
            {
                Debug.Log("¡Llegando al Jefe Final!");
                SceneManager.LoadScene("BossBattle");
                return; // Detener generación
            }
            // 1. Elegir un Chunk al azar de tu lista
            int randomIndex = Random.Range(0, chunkPrefabs.Length);
            LevelChunk prefabToSpawn = chunkPrefabs[randomIndex];

            // 2. Instanciarlo en la altura actual
            LevelChunk newChunk = Instantiate(prefabToSpawn, new Vector3(0, currentHeight, 0), Quaternion.identity, transform);
            
            // 3. Subir el puntero usando la altura que definiste en el script del Chunk
            currentHeight += newChunk.height;
        }

        // Poner la meta al final de todo
        if (goalPrefab != null)
        {
            Instantiate(goalPrefab, new Vector3(0, currentHeight + 2, 0), Quaternion.identity, transform);
        }
    }
}