using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LunesHelper
{
    public static string CrunchNumbers(float x)
    {
        float num = 0;
        if (x <= 1000)
        {
            num = x;
            return num.ToString("F0");
        }
        else if (x <= 1000000)
        {
            num = x / 1000;
            return num.ToString("F2") + "K";
        }
        else if (x <= 1000000000)
        {
            num = x / 1000000;
            return num.ToString("F2") + "M";
        }
        else if (x <= 1000000000000)
        {
            num = x / 1000000000;
            return num.ToString("F2") + "B";
        }
        else if (x <= 1000000000000000)
        {
            num = x / 1000000000000;
            return num.ToString("F2") + "T";
        }
        else if (x <= 1000000000000000000.0f)
        {
            num = x / 1000000000000000.0f;
            return num.ToString("F2") + "aa";
        }
        else if (x <= 1000000000000000000000.0f)
        {
            num = x / 1000000000000000000.0f;
            return num.ToString("F2") + "ab";
        }
        else if (x <= 1000000000000000000000000.0f)
        {
            num = x / 1000000000000000000000.0f;
            return num.ToString("F2") + "ac";
        }
        else if (x <= 1000000000000000000000000000.0f)
        {
            num = x / 1000000000000000000000000.0f;
            return num.ToString("F2") + "ad";
        }
        return num.ToString("F2");
    }
}
