using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CubeController;

[Serializable]
public class Player
{
    //Column, Line, Diagonal
    [HideInInspector]
    public Dictionary<string, int> DictionaryTableVariants = new Dictionary<string, int>();
    public PlayerType PlayerVariant;
    public bool IsBot;
    public static string _key(TableVariants variant, int numberLine, int numberColumn)
    {
        string result = variant.ToString();
        switch (variant)
        {
            case TableVariants.Line:
                result += numberLine.ToString();
                break;
            case TableVariants.Column:
                result += numberColumn.ToString();
                break;
            case TableVariants.Diagonal:
                result += numberLine.ToString() + numberColumn.ToString();
                break;
        }
        return result;
    }
    public enum PlayerType
    {
        X,
        O,
        None
    }
}
