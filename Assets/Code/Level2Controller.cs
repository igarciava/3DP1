using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Controller : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameController.GetGameController().SetPLayerLife(0.3f);

        }
        SceneManager.LoadSceneAsync("MainMenuScene");
    }
}
