using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージシーン固有の設定をここに入れる
public class StageSettings : MonoBehaviour {

    public PlayerMove.MoveType[] playerMoveTypeOnStart;
    public bool[] playerActiveOnStart = new bool[] { true, true };
    
}
