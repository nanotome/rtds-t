using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardManager;

    private void Awake()
    {
        // Ensure the instance is of the type GameManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Persist the GameManager instance across scenes
        DontDestroyOnLoad(gameObject);
    }
}
