using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heroes" , menuName = "Heroes/Create New Heroes")]
public class HeroBase : ScriptableObject
{
    [SerializeField] public string name;

    public enum AnimType
    {
        Warrior,
        Mage,
        Archer,
        Spear
    }

    [SerializeField] public AnimType heroType;

    [SerializeField] public float baseHP;

    [SerializeField] public float baseMP;

    [SerializeField] public int currentLevel;

    [SerializeField] public GameObject charPrefabs;
    [SerializeField] public GameObject charUI;
    [SerializeField] public GameObject charSlot;

    [SerializeField] public float physAttack;
    [SerializeField] public float magAttack;
    [SerializeField] public float physDefense;
    [SerializeField] public float magDefense;

    [SerializeField] public float critRate;
    [SerializeField] public float critDmg;

    [SerializeField] public float speed;

    [SerializeField] List<LearnableMovesHeroes> learnableMovesHeroes;

    [SerializeField] public GameObject face;
    public string Name
    {
        get { return name; }
    }
    public AnimType HeroType
    {
        get { return heroType; }
    }
    public int curLevel
    {
        get { return currentLevel; }
    }
    public float BaseHP
    {
        get { return baseHP; }
    }
    public float BaseMP
    {
        get { return baseMP; }
    }
    public GameObject CharPrefabs
    {
        get { return charPrefabs; }
    }
    public GameObject CharSlot
    {
        get { return charSlot; }
    }
    public GameObject Face
    {
        get { return face; }
    }
    public float PhysAttack
    {
        get { return physAttack; }
    }
    public float MagAttack
    {
        get { return magAttack; }
    }
    public float PhysDefense
    {
        get { return physDefense; }
    }
    public float MagDefense
    {
        get { return magDefense; }
    }
    public float CritRate
    {
        get { return critRate; }
    }
    public float CritDmg
    {
        get { return critDmg; }
    }
    public float Speed
    {
        get { return speed; }
    }
    public List<LearnableMovesHeroes> LearnableMovesHeroes
    {
        get { return learnableMovesHeroes; }
    }
    
}
[System.Serializable]
public class LearnableMovesHeroes
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }
    public int Level
    {
        get { return level; }
    }
}
