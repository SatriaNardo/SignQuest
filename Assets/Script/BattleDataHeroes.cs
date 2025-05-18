using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamData", menuName = "ScriptableObjects/TeamData", order = 1)]
public class BattleDataHeroes : ScriptableObject
{
    public List<HeroBase> currentHero = new List<HeroBase>();
}
