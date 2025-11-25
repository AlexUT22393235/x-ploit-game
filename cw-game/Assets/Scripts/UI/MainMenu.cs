using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject mainMenu;
    public GameObject roleMenu;
    public GameObject typeMenu;

    // Constantes para los nombres de las escenas y claves de PlayerPrefs
    private const string SelectedRoleKey = "SelectedRole";
    private const string IntroSceneName = "Intro"; // Nombre de tu escena de diálogo
    private const string TowerSceneName = "Tower"; // Nombre de tu escena de nivel
    private const string InfiniteSceneName = "Infitine"; // Nombre de tu escena de modo infinito
    
    // Constantes para el valor del rol
    public const int RoleKnight = 0;
    public const int RoleMage = 1;


    public void OpenOptionPanel()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    
    public void OpenMainMenuPanel()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        roleMenu.SetActive(false);
        typeMenu.SetActive(false);
    }
    
    public void OpenRolePanel()
    {
        mainMenu.SetActive(false);
        roleMenu.SetActive(true);
        typeMenu.SetActive(false);
    }

    public void OpenTypePanel()
    {
        roleMenu.SetActive(false);
        typeMenu.SetActive(true);
    }


    public void SelectKnight() 
    {
        // Guardar el rol como Caballero (0)
        PlayerPrefs.SetInt(SelectedRoleKey, RoleKnight); 
        Debug.Log("Rol seleccionado: Caballero");
        OpenTypePanel();
    }

    public void SelectMage() 
    {
        // Guardar el rol como Mago (1)
        PlayerPrefs.SetInt(SelectedRoleKey, RoleMage);
        Debug.Log("Rol seleccionado: Mago");
        OpenTypePanel();
    }


    public void StartStoryMode()
    {
        if (PlayerPrefs.HasKey(SelectedRoleKey))
        {
            // Cargar la escena de introducción (diálogo)
            SceneManager.LoadScene(IntroSceneName);
        }
        else
        {
            Debug.LogError("Error: No se ha seleccionado un rol.");
            OpenRolePanel(); 
        }
    }

    public void StartInfiniteMode() 
    {
        if (PlayerPrefs.HasKey(SelectedRoleKey))
        {
            // Cargar la escena de modo infinito
            SceneManager.LoadScene(InfiniteSceneName);
        }
        else
        {
            Debug.LogError("Error: No se ha seleccionado un rol.");
            OpenRolePanel(); 
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}