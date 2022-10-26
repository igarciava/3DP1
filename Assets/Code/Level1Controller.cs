using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour
{
    private void Start()
    {
        GameController.GetGameController().SetPLayerLife(1.0f);
        GameController.GetGameController().SetPLayerShield(1.0f);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadSceneAsync("Level 2 Scene");
        }
    }
}
