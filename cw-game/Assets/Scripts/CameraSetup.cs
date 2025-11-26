using UnityEngine;
using Unity.Cinemachine;

public class CameraSetup : MonoBehaviour
{
    void Start()
    {
        // 1. Obtenemos la referencia a la cámara en este mismo objeto
        var virtualCamera = GetComponent<CinemachineCamera>();

        // 2. Buscamos el objeto que contiene la clase 'Player'
        Player playerScript = FindAnyObjectByType<Player>();

        if (playerScript != null && virtualCamera != null)
        {
            // 3. Asignamos el Transform del player a los objetivos de la cámara
            virtualCamera.Follow = playerScript.transform;
            virtualCamera.LookAt = playerScript.transform; // Opcional: Si quieres que rote mirando al player

            Debug.Log("Player encontrado y asignado a la cámara.");
        }
        else
        {
            Debug.LogWarning("No se encontró el Player o la Cámara Virtual.");
        }
    }
}