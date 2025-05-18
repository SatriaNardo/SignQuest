using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies
{
    public EnemiesBase bases { get; set; }
    public int level { get; set; }

    public List<Moves> Move { get; set; }
    
    public float HP { get; set; }  
    public float ActionValue { get; set; }
    public Enemies(EnemiesBase eBase, int elevel)
    {
        bases = eBase;
        level = elevel;
        HP = MaxHP;
        ActionValue = Mathf.Floor(10000f / Speed);
        Move = new List<Moves>();
        foreach (var move in bases.LearnableMovesEnemies)
        {
            if (move.Level <= level)
                Move.Add(new Moves(move.Base));
        }
    }
    public float PhysAttack
    {
        get { return ((bases.physAttack * level) / 100f) + 5;}
    }
    public float MagAttack
    {
        get { return ((bases.MagAttack * level) / 100f) + 5; }
    }
    public float MagDefense
    {
        get { return ((bases.MagDefense * level) / 100f) + 5; }
    }
    public float PhysDefense
    {
        get { return ((bases.PhysDefense * level) / 100f) + 5; }
    }
    public float MaxHP
    {
        get { return ((bases.BaseHP * level) / 100f) + 10; }
    }
    public GameObject characters
    {
        get { return bases.CharPrefabs; }
    }
    public GameObject Faces
    {
        get { return bases.Face; }
    }
    public float Speed
    {
        get { return bases.speed; }
    }
    public bool TakeDamage(Moves move, Heroes attacker, float damageMultiplier)
    {
        float modifiers = Random.Range(0.9f, 1f);
        float attackStat = 0f;
        float defenseStat = 0f;

        switch (move.Base.AttacksType)
        {
            case MoveBase.AttackType.PhysicalAttack:
                attackStat = attacker.PhysAttack;
                defenseStat = PhysDefense;
                break;
            case MoveBase.AttackType.MagicAttack:
                attackStat = attacker.MagAttack;
                defenseStat = MagDefense;
                break;
            default:
                Debug.Log("This move type doesn't deal damage.");
                return false;
        }

        float a = (2 * attacker.level * 10f) / 250f;
        float d = a * move.Base.BasePower * (attackStat / defenseStat) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        damage = Mathf.FloorToInt(damage * damageMultiplier);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true; // Enemy fainted
        }
        return false;
    }

    public string TakeDamageNumbers(Moves move, Heroes attacker, float damageMultiplier)
    {
        float modifiers = Random.Range(0.9f, 1f);
        float attackStat = 0f;
        float defenseStat = 0f;

        switch (move.Base.AttacksType)
        {
            case MoveBase.AttackType.PhysicalAttack:
                attackStat = attacker.PhysAttack;
                defenseStat = PhysDefense;
                break;
            case MoveBase.AttackType.MagicAttack:
                attackStat = attacker.MagAttack;
                defenseStat = MagDefense;
                break;
            default:
                return "0";
        }

        float a = (2 * attacker.level * 10f) / 250f;
        float d = a * move.Base.BasePower * (attackStat / defenseStat) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        damage = Mathf.FloorToInt(damage * damageMultiplier);

        return damage.ToString();
    }

    public Moves GetRandomMoves()
    {
        int r = Random.Range(0, Move.Count);
        return Move[r];
    }
}
