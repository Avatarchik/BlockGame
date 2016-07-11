using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerSave
{
    public static int currentGridSize = 5;
    public static int currentLevel = 0;

    public static int[] maxLevel = new int[6];

    public static bool[] completedLevels4 = new bool[100];
    public static bool[] completedLevels5 = new bool[100];
    public static bool[] completedLevels6 = new bool[100];
}
