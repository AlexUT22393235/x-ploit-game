using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject mainMenu;
    public GameObject roleMenu;
    public GameObject typeMenu;

    private const string SelectedRoleKey = "SelectedRole";

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
        PlayerPrefs.SetInt(SelectedRoleKey, 0); 
        Debug.Log("Rol seleccionado: Caballero");
        OpenTypePanel();
    }

    public void SelectMage() 
    {
        PlayerPrefs.SetInt(SelectedRoleKey, 1);
        Debug.Log("Rol seleccionado: Mago");
        OpenTypePanel();
    }


    public void StartStoryMode()
    {
        if (PlayerPrefs.HasKey(SelectedRoleKey))
        {
            SceneManager.LoadScene("Tower");
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
            SceneManager.LoadScene("Infitine");
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