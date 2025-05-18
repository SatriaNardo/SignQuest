using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies" , menuName = "Enemies/Create New Enemies")]

public class EnemiesBase : ScriptableObject
{
    [SerializeField] public string name;

    public enum Type
    {
        UNDEAD,
        BEAST,
        DRAGON,
        HUMANOID,
        CONSTRUCT
    }
    public enum Rarity
    {
        COMMON,
        ELITE,
        BOSS
    }
    [SerializeField] public Type enemyType;
    [SerializeField] public Rarity rarity;

    [SerializeField] public float baseHP;

    [SerializeField] public GameObject charPrefabs;

    [SerializeField] public float physAttack;
    [SerializeField] public float magAttack;
    [SerializeField] public float physDefense;
    [SerializeField] public float magDefense;

    [SerializeField] public float critRate;
    [SerializeField] public float critDmg;

    [SerializeField] public float speed;

    [SerializeField] public GameObject face;

    [SerializeField] List<LearnableMovesEnemies> learnableMovesEnemies;
    [SerializeField] public List<EnemyDropItem> drops;
    public List<EnemyDropItem> Drops => drops;
    public string Name
    {
        get { return name; }
    }
    public Type EnemyType
    {
        get { return enemyType; }
    }
    public Rarity Rarities
    {
        get { return rarity; }
    }
    public float BaseHP
    {
        get { return baseHP; }
    }
    public GameObject CharPrefabs
    {
        get { return charPrefabs; }
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
        get { return magDefense;  }
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
        get { return speed;  }
    }
    public List<LearnableMovesEnemies> LearnableMovesEnemies
    {
        get { return learnableMovesEnemies; }
    }


}
[System.Serializable]
public class LearnableMovesEnemies
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
[System.Serializable]
public class EnemyDropItem
{
    public InventoryItem item; // reference to the InventoryItem ScriptableObject
    [Range(0f, 1f)] public float dropChance = 1f;
}

