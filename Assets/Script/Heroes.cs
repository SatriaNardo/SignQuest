using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Heroes
{
    public HeroBase bases { get; set; }
    public int level { get; set; }

    public float HP { get; set; }

    public float MP { get; set; }
    public float ActionValue { get; set; }
    public float ResetAV { get; set; }
    public List<Moves> Move { get; set; }

    public Heroes(HeroBase ebase, int elevel)
    {
        bases = ebase;
        level = elevel;
        HP = MaxHP;
        MP = MaxMP;
        ActionValue = Mathf.Floor(10000f / Speed);
        Move = new List<Moves>();
        foreach (var move in bases.LearnableMovesHeroes)
        {
            if (move.Level <= level)
                Move.Add(new Moves(move.Base));
        }
    }
    public float PhysAttack
    {
        get { return ((bases.physAttack * level) / 100f) + 5; }
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
    public float MaxMP
    {
        get { return ((bases.BaseMP * level) / 100f) + 5; }
    }
    public float Speed
    {
        get { return bases.speed;}
    }
    public GameObject characters
    {
        get { return bases.CharPrefabs; }
    }
    public GameObject CharactersOverworld
    {
        get { return bases.CharSlot; }
    }
    public GameObject Faces
    {
        get { return bases.Face; }
    }
   
    public bool TakeDamage(Moves move, Enemies attacker)
    {
        float modifiers = Random.Range(0.9f, 1f);
        float attackStat = 0f;
        float defenseStat = 0f;

        // Choose stats based on the move type
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

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true; // Fainted
        }
        return false;
    }
    public void Heal(float amount)
    {
        HP += Mathf.FloorToInt(amount);
        HP = Mathf.Clamp(HP, 0, MaxHP);
    }

}
