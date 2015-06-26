using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameScene
{
    private static GameObject _start;
    private static GameObject _main;
    private static GameObject _end;
    public static void init(GameObject start,GameObject main,GameObject end)
    {
        _start = start;
        _main = main;
        _end = end;
    }

    public static void GotoScene(int index)
    {
        switch(index)
        {
            case 1:
                Time.timeScale = 0;
                _start.SetActive(true);
                _main.SetActive(false);
                _end.SetActive(false);
                break;
            case 2:
                Time.timeScale = 1;
                _start.SetActive(false);
                _main.SetActive(true);
                _main.SendMessage("Reset");
                _end.SetActive(false);
                break;
            case 3:
                _start.SetActive(false);
                _main.SetActive(false);
                _end.SetActive(true);
                break;
        }
    }
}

