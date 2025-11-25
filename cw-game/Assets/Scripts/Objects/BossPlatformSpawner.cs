using UnityEngine;
using System.Collections;

public class BossPlatformSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject normalPlatform;
    public GameObject timedPlatform; // La que desaparece

    [Header("Configuración")]
    public float waterLevelY = -5f; // Dónde nacen (bajo el agua)
    public float floorHeightY = -1f; // A qué altura se detienen (el suelo)
    public int arenaWidth = 5;       // Cuántas plataformas de ancho
    
    [Header("Distribución de Plataformas")]
    public float leftEdge = -1.5f;      // Borde izquierdo en unidades de mundo
    public float rightEdge = 1.5f;      // Borde derecho en unidades de mundo
    public float platformSpacing = 0.6f; // Separación entre plataformas
    public bool autoCalculateEdges = true; // Si true, calcula left/right según cámara
    public float platformWidth = 0.6f; // Ancho aproximado del sprite de la plataforma
    public float edgePadding = 0.05f; // Pequeño padding desde el borde de cámara

    void Start()
    {
        // Calcular bordes automáticamente si está activado
        if (autoCalculateEdges)
        {
            CalculateEdges();
        }

        StartCoroutine(SpawnArenaSequence());
    }

    void CalculateEdges()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("BossPlatformSpawner: Camera.main no encontrada. No se calculan los bordes.");
            return;
        }

        // Para cámara ortográfica: la mitad de la altura es orthographicSize
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float camCenterX = cam.transform.position.x;

        // Dejar que las plataformas queden dentro de la vista, con padding
        float halfPlatform = platformWidth * 0.5f;
        leftEdge = camCenterX - halfWidth + halfPlatform + edgePadding;
        rightEdge = camCenterX + halfWidth - halfPlatform - edgePadding;

        // Si platformSpacing no está definido, usar platformWidth como separación
        if (platformSpacing <= 0f)
            platformSpacing = platformWidth;
    }

    IEnumerator SpawnArenaSequence()
    {
        yield return new WaitForSeconds(0.1f);

        // Distribución configurable mediante `leftEdge`, `rightEdge` y `platformSpacing` en el Inspector
        for (float x = leftEdge; x <= rightEdge; x += platformSpacing)
        {
            // Elegir tipo de plataforma (30% probabilidad de ser de tiempo)
            GameObject prefabToUse = (Random.value > 0.7f) ? timedPlatform : normalPlatform;

            // Posición de nacimiento (Bajo el agua)
            Vector3 spawnPos = new Vector3(x, waterLevelY, 0);

            // Crear la plataforma
            GameObject newPlat = Instantiate(prefabToUse, spawnPos, Quaternion.identity);

            // Configurar hasta dónde debe subir
            EmergingPlatform script = newPlat.GetComponent<EmergingPlatform>();
            if (script != null)
            {
                script.Setup(floorHeightY);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}