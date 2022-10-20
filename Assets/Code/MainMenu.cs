using UnityEngine;
using UnityEngine.SceneManagement;
//public class something : MonoBehaviour
//{
//    //clic encima del nombre de la clase > ctrl + .
//    //mover tipo a "nombre de la clase".cs
//}

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameController.DestroySingleton();
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnStartClicked()
    {
        SceneManager.LoadSceneAsync("Level 1 Scene");
    }
}
