using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { InGame, GridSelector, LevelSelector, LevelGenerator}

public static class StateMachine
{
    public static int currentGridSize = 6;
    public static int currentLevel = 3;

    public static GameState state =  GameState.InGame;
}
