using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossSceneInfo
{
    public static bool restart = false;
    public static bool restartToEmails = false;
    public static bool victoryEmails = false;
    public static bool playingMusic = false;
    public static FMOD.Studio.EventInstance musicInst;
    public static FMOD.Studio.EventInstance batteryOvertimeInst;

    public static List<bool> readEmails = new List<bool>();
}
