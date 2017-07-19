using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    private BoardManager boardScript;
    private Player player;

    public int level = 0;
    public bool loading = true;

    public float playerHealth;
    public float playerXP;

    private void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        level++;
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
        // store player stats
        playerHealth = player.GetHealth();
        playerXP = player.GetXP();

        boardScript.DestroyLevel();
        SceneManager.LoadScene(0);
    }
}
