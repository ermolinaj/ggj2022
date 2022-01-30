using UnityEngine;

public class Menu : MonoBehaviour
{
    
    public void StartGame()
    {
        SceneController.LoadScene("PeopleSpawningPrototype", 1, 1);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
