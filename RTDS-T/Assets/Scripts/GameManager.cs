﻿using UnityEngine;
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
        player.OnDeath += GameOver;

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
        // compute new health as fraction of xp
        float currentHealth = player.GetHealth();
        float currentXP = player.GetXP();

        float remainingXP = currentXP % 5;
        float healthGain = currentXP / 5;

        // store player stats
        playerHealth = currentHealth + healthGain;
        playerXP = remainingXP;

        boardScript.DestroyLevel();
        SceneManager.LoadScene(0);
    }

    void GameOver()
    {
        enabled = false;
    }
}
