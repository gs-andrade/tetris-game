using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Text ScoreText;
    public PauseMenu PauseMenu;

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void ToogleMenu(bool toogle, MenuType menuType)
    {
        PauseMenu.ToogleMenu(toogle, menuType);
    }

}
