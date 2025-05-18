using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill/Magic", menuName = "Characters/Create new skill or magic")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    public enum AttackType
    {
        PhysicalAttack,
        MagicAttack,
        Buffing,
        Healing
    }
    public enum AttackSize
    {
        ST,
        AOE
    }

    [SerializeField] public AttackType attackType;
    [SerializeField] float basePower;
    [SerializeField] float mpComsumption;
    [SerializeField] int timeCasting;
    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public AttackType AttacksType
    {
        get { return attackType; }
    }
    public float BasePower
    {
        get { return basePower; }
    }
    public float MPComsumption
    {
        get { return mpComsumption; }
    }
    public int TimeCasting
    {
        get { return timeCasting; }
    }
}
