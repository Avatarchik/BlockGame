using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { InGame, GridSelector, LevelSelector, LevelGenerator, LevelCreator}

public static class StateMachine
{
    public static GameState state = GameState.InGame;

    public static int currentGridSize = 5;
    public static int currentLevel = 0;
}
