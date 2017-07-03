using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static bool GameOver;

    public GameObject gameOverUI;

	// Update is called once per frame
	void Update () {
        if (GameOver)
            return;

	    if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
	}

    void Start()
    {
        GameOver = false;
    }

    void EndGame()
    {
        GameOver = true;

        gameOverUI.SetActive(true);
    }
}
