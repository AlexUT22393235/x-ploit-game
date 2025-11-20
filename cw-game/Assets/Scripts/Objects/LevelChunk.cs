using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    [Header("Configuración")]
    public float height = 10f; // ¿Qué tan alto es este tramo? (Mídelo en Unity)

    [Header("Variaciones Aleatorias")]
    public GameObject[] optionalObjects; // Arrastra aquí cosas que pueden NO aparecer
    public Transform[] movingPlatforms;  // Plataformas que quieres mover un poco

    void Start()
    {
        ApplyRandomness();
    }

    void ApplyRandomness()
    {
        // 1. Elementos que aparecen o desaparecen (50% probabilidad)
        foreach (GameObject obj in optionalObjects)
        {
            bool keep = Random.value > 0.5f; // Cara o cruz
            obj.SetActive(keep);
        }

        // 2. Mover ligeramente algunas plataformas (Variación de posición)
        foreach (Transform plat in movingPlatforms)
        {
            // Mover entre -1 y 1 unidad en X sobre su posición original
            float randomOffset = Random.Range(-1f, 1f);
            plat.localPosition = new Vector3(plat.localPosition.x + randomOffset, plat.localPosition.y, plat.localPosition.z);
        }
    }

    // Dibujar una linea amarilla en el editor para ver cuanto mide el chunk
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.up * (height / 2), new Vector3(10, height, 0));
    }
}