using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameplayController gameController;
    private MenuController menuController;

    private GameState state;

    private int currentScore;
    private void Awake()
    {
        gameController = GetComponentInChildren<GameplayController>();

        //TODO: COMPLETAR SISTEMA DE MENU
        menuController = GetComponentInChildren<MenuController>();

        gameController.Setup();

        state = GameState.Menu;
    }

    public void StartGame()
    {
        gameController.ResetGame();
        state = GameState.Gameplay;
        menuController.ToogleMenu(false, MenuType.None);
        menuController.UpdateScore(0);
    }

    public void ResumeGame()
    {
        menuController.ToogleMenu(false, MenuType.None);
        state = GameState.Gameplay;
    }

    public void UnpauseGame()
    {
        menuController.ToogleMenu(false, MenuType.None);
        state = GameState.Gameplay;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Menu:
                {
                    break;
                }

            case GameState.Gameplay:
                {
                    state = gameController.UpdateGame();

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                       menuController.ToogleMenu(true, MenuType.PauseMenu);
                        state = GameState.Paused;
                    }

                    break;
                }

            case GameState.IncreaseScore:
                {
                    int linesDestroyed = gameController.LinesDestroyed;
                    linesDestroyed--;

                    currentScore += 100 + ( 200 * linesDestroyed);

                    menuController.UpdateScore(currentScore);
                    state = GameState.Gameplay;

                    break;
                }

            case GameState.Paused:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        menuController.ToogleMenu(false, MenuType.None);
                        state = GameState.Gameplay;
                    }

                    break;
                }

            case GameState.GameOver:
                {
                    menuController.ToogleMenu(true, MenuType.GameOverMenu);
                    state = GameState.Menu;
                    break;
                }
        }

    }
}

public enum GameState
{
    Menu,
    Gameplay,
    IncreaseScore,
    Paused,
    GameOver,
}