using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    float originalHpScale = 0.6f;
    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized * originalHpScale, health.transform.localScale.y);
    }
    public IEnumerator SetHpSmooth(float newHp)
    {
        float targetHp = newHp * originalHpScale;
        float curHp = health.transform.localScale.x;

        while (Mathf.Abs(curHp - targetHp) > 0.001f)
        {
            curHp = Mathf.MoveTowards(curHp, targetHp, Time.deltaTime * 2f); // adjust speed here
            health.transform.localScale = new Vector3(curHp, health.transform.localScale.y, health.transform.localScale.z);
            yield return null;
        }

        health.transform.localScale = new Vector3(targetHp, health.transform.localScale.y, health.transform.localScale.z);
    }


}
