using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public GameObject knightCharacter; // Arrastra aquí el GameObject del Caballero
    public GameObject mageCharacter;   // Arrastra aquí el GameObject del Mago

    private const string SelectedRoleKey = "SelectedRole";

    void Start()
    {
        LoadAndActivateRole();
    }

    void LoadAndActivateRole()
    {
        // 1. Verificar si hay un rol guardado
        if (PlayerPrefs.HasKey(SelectedRoleKey))
        {
            int selectedRole = PlayerPrefs.GetInt(SelectedRoleKey);

            // 2. Desactivar ambos personajes por defecto
            knightCharacter.SetActive(false);
            mageCharacter.SetActive(false);
            
            // 3. Activar el personaje seleccionado
            if (selectedRole == 0) // 0 es Caballero
            {
                knightCharacter.SetActive(true);
                Debug.Log("Iniciando juego como Caballero.");
            }
            else if (selectedRole == 1) // 1 es Mago
            {
                mageCharacter.SetActive(true);
                Debug.Log("Iniciando juego como Mago.");
            }
            else
            {
                Debug.LogError("Valor de rol guardado inválido: " + selectedRole);
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún rol guardado. Cargando Caballero por defecto.");
            // Cargar un personaje por defecto si no se encuentra el valor
            knightCharacter.SetActive(true);
            mageCharacter.SetActive(false);
        }
    }
}