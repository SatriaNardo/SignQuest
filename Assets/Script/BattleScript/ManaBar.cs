using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    [SerializeField] GameObject mana;

    public void SetMP(float mpNormalized)
    {
        mana.transform.localScale = new Vector3(mpNormalized * mana.transform.localScale.x, mana.transform.localScale.y);
    }
}
