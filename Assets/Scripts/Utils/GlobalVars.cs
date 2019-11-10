using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : Singleton<GlobalVars>
{

    protected GlobalVars() { }

    public string MainScene = "Rivendell";
    public string EndScene = "Fireworks";

}