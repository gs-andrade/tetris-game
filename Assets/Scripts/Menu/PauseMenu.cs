using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PauseMenu : MonoBehaviour
{
    public float DistanceToMove;
    public float DistanceToReturn;

    public GameObject MainMenuObj;
    public GameObject PauseMenuObj;
    public GameObject GameOverMenuObj;
    public GameObject PauseButton;
    public void ToogleMenu(bool toogle, MenuType menuType)
    {
        transform.DOKill();

        if (toogle)
        {
            transform.DOMoveX(DistanceToMove, 1f);
        }
        else
            transform.DOMoveX(DistanceToReturn, 1f);

        GameObject menuToActive = null;

        switch (menuType)
        {
            case MenuType.None:
                {
                    menuToActive = PauseButton;
                    break;
                }

            case MenuType.MainMenu:
                menuToActive = MainMenuObj;
                break;

            case MenuType.PauseMenu:
                menuToActive = PauseMenuObj;
                break;

            case MenuType.GameOverMenu:
                menuToActive = GameOverMenuObj;
                break;
        }

        MainMenuObj.SetActive(false);
        PauseMenuObj.SetActive(false);
        GameOverMenuObj.SetActive(false);
        PauseButton.SetActive(false);


        if (menuToActive != null)
            menuToActive.SetActive(true);

    }
    //OBS: IDEALMENTE, USARIA INTERFACE PARA PADRONIZAR PARA TODOS OS MENUS EXISTENTES DO JOGO
}

public enum MenuType
{
    None,
    MainMenu,
    PauseMenu,
    GameOverMenu,
}