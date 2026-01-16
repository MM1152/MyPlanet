using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PresetWindowTutorial3 : Tutorial
{
    public override void TutorialExit()
    {
        base.TutorialExit();
        FirebaseManager.Instance.UserData.isClearPresetTutorial = true;
        FirebaseManager.Instance.UserData.ClearPresetTutorial().Forget();
    }
}