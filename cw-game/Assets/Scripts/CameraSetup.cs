using UnityEngine;
using Unity.Cinemachine;
using System.Collections; // Necesario para IEnumerator

public class CameraSetup : MonoBehaviour
{
    IEnumerator Start()
    {
        // Esperamos un frame. Esto permite que otros scripts (como el del Player)
        // terminen su Awake/Start antes de buscarlo.
        yield return null; 

        var virtualCamera = GetComponent<CinemachineCamera>();
        
        // Usamos FindFirstObjectByType que es ligeramente más rápido/moderno que FindAny
        Player playerScript = FindFirstObjectByType<Player>();

        if (playerScript != null && virtualCamera != null)
        {
            virtualCamera.Follow = playerScript.transform;
            virtualCamera.LookAt = playerScript.transform; 
            Debug.Log("Player encontrado y asignado.");
        }
        else
        {
            // Si falla, intentamos buscarlo de nuevo en un segundo (reintento)
            Debug.LogWarning("Reintentando búsqueda del player...");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Start()); 
        }
    }
}