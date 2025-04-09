using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
      private EventBinding<ChangeScene> changeSceneEventBinding;
      private void OnEnable()
      {
            changeSceneEventBinding = new EventBinding<ChangeScene>((e) =>
            {
                  LoadScene(e.sceneIndex);
            });
            EventBus<ChangeScene>.Register(changeSceneEventBinding);
      }

      private void OnDisable() => EventBus<ChangeScene>.Deregister(changeSceneEventBinding);
      

      public void LoadScene(int sceneNumber)
      {
            Time.timeScale = 1;
            if (sceneNumber >= SceneManager.sceneCountInBuildSettings) return;
            SceneManager.LoadScene(sceneNumber);
      }

      public void QuitGame()
      {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
      }
}

