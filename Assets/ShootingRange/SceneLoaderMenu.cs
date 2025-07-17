using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Замените на имя вашей сцены меню
    }
}