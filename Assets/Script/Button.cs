using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("패널")]
    public GameObject helpPanel;
    public GameObject leaderPanel;

    private void Start()
    {
        // 시작할 때 두 패널 모두 숨기기
        if (helpPanel != null) helpPanel.SetActive(false);
        if (leaderPanel != null) leaderPanel.SetActive(false);
    }

    // ==================== 게임 시작 ====================
    public void GoToGame()
    {
        SceneManager.LoadScene("Race");   // ← 게임 씬 이름으로 변경하세요
    }

    // ==================== 리더보드 ====================
    public void OpenLeader()
    {
        if (leaderPanel != null)
            leaderPanel.SetActive(true);
    }

    public void CloseLeader()
    {
        if (leaderPanel != null)
            leaderPanel.SetActive(false);
    }

    // ==================== 도움말 ====================
    public void OpenHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        if (helpPanel != null)
            helpPanel.SetActive(false);
    }

    // ==================== 나가기 ====================
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}