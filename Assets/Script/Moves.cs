using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves 
{
    public MoveBase Base { get; set; }
    public int turn { get; set; }

    public Moves(MoveBase pBase)
    {
        Base = pBase;
        turn = pBase.TimeCasting;
    }
}
