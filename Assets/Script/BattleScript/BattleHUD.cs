using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText1Hero;
    [SerializeField] TextMeshProUGUI levelText2Hero;
    [SerializeField] TextMeshProUGUI levelText3Hero;
    [SerializeField] HpBar hpBar1Hero;
    [SerializeField] HpBar hpBar2Hero;
    [SerializeField] HpBar hpBar3Hero;
    [SerializeField] ManaBar manaBar1Hero;
    [SerializeField] ManaBar manaBar2Hero;
    [SerializeField] ManaBar manaBar3Hero;
    [SerializeField] TextMeshProUGUI levelText1Enemies;
    [SerializeField] TextMeshProUGUI levelText2Enemies;
    [SerializeField] TextMeshProUGUI levelText3Enemies;
    [SerializeField] HpBar hpBar1Enemies;
    [SerializeField] HpBar hpBar2Enemies;
    [SerializeField] HpBar hpBar3Enemies;
    List<HpBar> hpBarsHeroes = new List<HpBar>();
    List<HpBar> hpBarsEnemies = new List<HpBar>();
    Heroes _heroes1;
    Enemies _enemies1;
    Heroes _heroes2;
    Enemies _enemies2;
    Heroes _heroes3;
    Enemies _enemies3;
    public void SetDataEnemies1(Enemies enemies)
    {
        _enemies1 = enemies;
        levelText1Enemies.text = "Lvl. " + enemies.level;
        hpBar1Enemies.SetHP((float)enemies.HP / enemies.MaxHP);
        hpBarsEnemies.Add(hpBar1Enemies);
    }
    public void SetDataEnemies2(Enemies enemies)
    {
        _enemies2 = enemies;
        levelText2Enemies.text = "Lvl. " + enemies.level;
        hpBar2Enemies.SetHP((float)enemies.HP / enemies.MaxHP);
        hpBarsEnemies.Add(hpBar2Enemies);
    }
    public void SetDataEnemies3(Enemies enemies)
    {
        _enemies3 = enemies;
        levelText3Enemies.text = "Lvl. " + enemies.level;
        hpBar3Enemies.SetHP((float)enemies.HP / enemies.MaxHP);
        hpBarsEnemies.Add(hpBar3Enemies);
    }
    public void SetDataHeroes1(Heroes heroes)
    {
        _heroes1 = heroes;
        levelText1Hero.text = "Lvl. " + heroes.level;
        hpBar1Hero.SetHP((float)heroes.HP / heroes.MaxHP);
        manaBar1Hero.SetMP((float)heroes.MP / heroes.MaxMP);
        hpBarsHeroes.Add(hpBar1Hero);
    }
    public void SetDataHeroes2(Heroes heroes)
    {
        _heroes2 = heroes;
        levelText2Hero.text = "Lvl. " + heroes.level;
        hpBar2Hero.SetHP((float)heroes.HP / heroes.MaxHP);
        manaBar2Hero.SetMP((float)heroes.MP / heroes.MaxMP);
        hpBarsHeroes.Add(hpBar2Hero);
    }
    public void SetDataHeroes3(Heroes heroes)
    {
        _heroes3 = heroes;
        levelText3Hero.text = "Lvl. " + heroes.level;
        hpBar3Hero.SetHP((float)heroes.HP / heroes.MaxHP);
        manaBar3Hero.SetMP((float)heroes.MP / heroes.MaxMP);
        hpBarsHeroes.Add(hpBar3Hero);
    }
    public IEnumerator UpdateHpHeroes(int i, Heroes curHeroes)
    {
        yield return hpBarsHeroes[i].SetHpSmooth((float)curHeroes.HP / curHeroes.MaxHP);
    }
    public IEnumerator UpdateHpEnemies(int i, Enemies curEnemies)
    {
        yield return hpBarsEnemies[i].SetHpSmooth((float)curEnemies.HP / curEnemies.MaxHP);
    }
    public void DestroyBarsEnemies(int i)
    {
        hpBarsEnemies.RemoveAt(i);
    }
    public void DestroyBarsHeroes(int i)
    {
        hpBarsHeroes.RemoveAt(i);
    }
    public IEnumerator UpdateHPHero1()
    {
        yield return hpBar1Hero.SetHpSmooth((float)_heroes1.HP / _heroes1.MaxHP);
    }
    public IEnumerator UpdateHPEnemy1()
    {
        yield return hpBar1Enemies.SetHpSmooth((float)_enemies1.HP / _enemies1.MaxHP);
    }
    public IEnumerator UpdateHPHero2()
    {
        yield return hpBar2Hero.SetHpSmooth((float)_heroes2.HP / _heroes2.MaxHP);
    }
    public IEnumerator UpdateHPEnemy2()
    {
        yield return hpBar2Enemies.SetHpSmooth((float)_enemies2.HP / _enemies2.MaxHP);
    }
    public IEnumerator UpdateHPHero3()
    {
        yield return hpBar3Hero.SetHpSmooth((float)_heroes3.HP / _heroes3.MaxHP);
    }
    public IEnumerator UpdateHPEnemy3()
    {
        yield return hpBar3Enemies.SetHpSmooth((float)_enemies3.HP / _enemies3.MaxHP);
    }
}
