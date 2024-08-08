using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void CharCards()
    {
        SceneManager.LoadScene("CardsMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
