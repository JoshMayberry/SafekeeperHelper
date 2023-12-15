using System.Collections;
using UnityEngine;

using Aarthificial.Safekeeper;
using Aarthificial.Safekeeper.Loaders;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

public class SaveManager : MonoBehaviour {
    private SaveControllerBase _controller;

    public static SaveManager instance { get; private set; }

    private void Awake() {
        Debug.Log("@SaveManager.Awake");
        if (instance != null) {
            Debug.LogError("Found more than one SaveManager in the scene.");
        }

        instance = this;

        _controller = new SaveControllerBase(
          new FileSaveLoader("slot1")
        );
        _controller.Initialize();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadScene(string scenePath) {
        Debug.Log("@SaveManager.LoadScene");
        // Save the current state to memory
        yield return WaitForTask(_controller.Save());
        // Switch the scene
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scenePath);
        // Load the state from memory
        yield return WaitForTask(_controller.Load());
    }

    public IEnumerator SaveGame() {
        Debug.Log("@SaveManager.SaveGame");
        // Save the current state to memory and commit it to the persistent storage
        yield return WaitForTask(_controller.Save(SaveMode.Full));
    }

    public IEnumerator LoadGame() {
        Debug.Log("@SaveManager.LoadGame.1");
        // Switch the scene
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
          UnityEngine.SceneManagement.SceneManager.GetActiveScene().path
        );
        Debug.Log("@SaveManager.LoadGame.2");
        // Load the state from memory
        yield return WaitForTask(_controller.Load());
    }

    public IEnumerator ResetGame() {
        Debug.Log("@SaveManager.ResetGame");
        // Reset memory with the data from the persistent storage
        yield return WaitForTask(_controller.Load(SaveMode.PersistentOnly));
        // Reload the scene
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
          UnityEngine.SceneManagement.SceneManager.GetActiveScene().path
        );
        // Load the state from memory
        yield return WaitForTask(_controller.Load(SaveMode.Full));
    }

    private IEnumerator WaitForTask(Task task) {
        while (!task.IsCompleted) {
            yield return null;
        }

        if (task.IsFaulted) {
            ExceptionDispatchInfo.Capture(task.Exception).Throw();
        }
    }
}
