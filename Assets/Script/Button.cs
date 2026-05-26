using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject leaderPanel;


    private void Start() // 뒤에
    {
        helpPanel.SetActive(false);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Race");
    }

    public void OpenLeader()
    {
        leaderPanel.SetActive(true);
    }
    public void CloseLeader()
    {
        leaderPanel.SetActive(false);
    }

    public void OpenHelp()
    {
        helpPanel.SetActive(true);
    }
    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}