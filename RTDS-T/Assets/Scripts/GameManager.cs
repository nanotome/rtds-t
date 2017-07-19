using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    private BoardManager boardScript;

    public int level = 0;
    public bool loading = true;

    private void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        // Ensure the instance is of the type GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Persist the GameManager instance across scenes
        DontDestroyOnLoad(gameObject);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level loaded");
        level++;
        Debug.Log(level);
        loading = true;
        // TODO: show a loading banner here
        boardScript.SetUpLevel(level);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void Restart()
    {
        boardScript.DestroyLevel();
        SceneManager.LoadScene(0);
    }
}
