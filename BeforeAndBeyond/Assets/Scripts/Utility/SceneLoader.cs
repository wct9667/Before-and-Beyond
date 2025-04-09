using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{

      public static void LoadScene(int sceneNumber)
      {
            Time.timeScale = 1;
            if (sceneNumber >= SceneManager.sceneCountInBuildSettings) return;
            SceneManager.LoadScene(sceneNumber);
      }

      public static void QuitGame()
      {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
      }
}

