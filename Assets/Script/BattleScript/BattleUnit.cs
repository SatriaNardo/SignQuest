using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] BattleDataHeroes heroTeams;
    [SerializeField] HeroBase baseHero1;
    public bool hero1 = false;
    [SerializeField] HeroBase baseHero2;
    public bool hero2 = false;
    [SerializeField] HeroBase baseHero3;
    public bool hero3 = false;
    [SerializeField] HeroBase EmptyCheckHero;
    [SerializeField] HeroBase defaultHero;
    [SerializeField] int levelHero1;
    [SerializeField] int levelHero2;
    [SerializeField] int levelHero3;
    [SerializeField] GameObject spawnPositionHero1;
    [SerializeField] GameObject spawnPositionHero2;
    [SerializeField] GameObject spawnPositionHero3;
    List<GameObject> spawnListHeroes = new List<GameObject>();
    [SerializeField] EnemiesBase baseEnemies1;
    public bool enemy1 = false;
    [SerializeField] EnemiesBase baseEnemies2;
    public bool enemy2 = false;
    [SerializeField] EnemiesBase baseEnemies3;
    public bool enemy3 = false;
    [SerializeField] EnemiesBase EmptyCheckEnemy;
    [SerializeField] int levelEnemies1;
    [SerializeField] int levelEnemies2;
    [SerializeField] int levelEnemies3;
    [SerializeField] GameObject spawnPositionEnemies1;
    [SerializeField] GameObject spawnPositionEnemies2;
    [SerializeField] GameObject spawnPositionEnemies3;
    List<GameObject> spawnListEnemies = new List<GameObject>();
    [SerializeField] public GameObject enemiesHUD1;
    [SerializeField] public GameObject enemiesHUD2;
    [SerializeField] public GameObject enemiesHUD3;
    List<GameObject> enemiesHUDs = new List<GameObject>();
    [SerializeField] public GameObject playerHUD1;
    [SerializeField] public GameObject playerHUD2;
    [SerializeField] public GameObject playerHUD3;
    [SerializeField] BattleSystem battleSystem;
    List<GameObject> playerHUDs = new List<GameObject>();
    EnemyManagers enemyTeams = EnemyManagers.Instance;
    
    public Heroes heroes1 { get; set; }
    public Heroes heroes2 { get; set; }
    public Heroes heroes3 { get; set; }

    public Enemies enemies1 { get; set; }
    public Enemies enemies2 { get; set; }
    public Enemies enemies3 { get; set; }

    public void Setup()
    {
        if (heroTeams.currentHero[0] == EmptyCheckHero)
        {
            if (heroTeams.currentHero[1] != EmptyCheckHero)
            {
                heroTeams.currentHero[0] = heroTeams.currentHero[1];
                heroTeams.currentHero[1] = EmptyCheckHero;
                heroes1 = new(heroTeams.currentHero[0], heroTeams.currentHero[0].curLevel);
                Instantiate(heroes1.characters, spawnPositionHero1.transform.position, Quaternion.identity, spawnPositionHero1.transform);
                spawnListHeroes.Add(spawnPositionHero1);
                playerHUDs.Add(playerHUD1);
            }
            else if(heroTeams.currentHero[2] != EmptyCheckHero)
            {
                heroTeams.currentHero[0] = heroTeams.currentHero[2];
                heroTeams.currentHero[2] = EmptyCheckHero;
                heroes1 = new(heroTeams.currentHero[0], heroTeams.currentHero[0].curLevel);
                Instantiate(heroes1.characters, spawnPositionHero1.transform.position, Quaternion.identity, spawnPositionHero1.transform);
                spawnListHeroes.Add(spawnPositionHero1);
                playerHUDs.Add(playerHUD1);
            }
            else
            {
                heroTeams.currentHero[0] = defaultHero;
                heroes1 = new(heroTeams.currentHero[0], heroTeams.currentHero[0].curLevel);
                Instantiate(heroes1.characters, spawnPositionHero1.transform.position, Quaternion.identity, spawnPositionHero1.transform);
                spawnListHeroes.Add(spawnPositionHero1);
                playerHUDs.Add(playerHUD1);
            }
        }
        else
        {
            heroes1 = new(heroTeams.currentHero[0], heroTeams.currentHero[0].curLevel);
            Instantiate(heroes1.characters, spawnPositionHero1.transform.position, Quaternion.identity, spawnPositionHero1.transform);
            spawnListHeroes.Add(spawnPositionHero1);
            playerHUDs.Add(playerHUD1);
        }
        if (heroTeams.currentHero[1] == EmptyCheckHero)
        {
            if(heroTeams.currentHero[2] != EmptyCheckHero)
            {
                heroTeams.currentHero[1] = heroTeams.currentHero[2];
                heroTeams.currentHero[2] = EmptyCheckHero;
                heroes2 = new(heroTeams.currentHero[1], heroTeams.currentHero[1].curLevel);
                Instantiate(heroes2.characters, spawnPositionHero2.transform.position, Quaternion.identity, spawnPositionHero2.transform);
                spawnListHeroes.Add(spawnPositionHero2);
                playerHUDs.Add(playerHUD2);
            }
            else
            {
                hero2 = false;
            }
        }
        else
        {
            heroes2 = new(heroTeams.currentHero[1], heroTeams.currentHero[1].curLevel);
            Instantiate(heroes2.characters, spawnPositionHero2.transform.position, Quaternion.identity, spawnPositionHero2.transform);
            spawnListHeroes.Add(spawnPositionHero2);
            playerHUDs.Add(playerHUD2);
        }
        if (heroTeams.currentHero[2] == EmptyCheckHero)
        {
            hero3 = false;
        }
        else
        {
            heroes3 = new(heroTeams.currentHero[2], heroTeams.currentHero[2].curLevel);
            Instantiate(heroes3.characters, spawnPositionHero3.transform.position, Quaternion.identity, spawnPositionHero3.transform);
            spawnListHeroes.Add(spawnPositionHero3);
            playerHUDs.Add(playerHUD3);
        }
        


        if (enemyTeams.ActiveEnemies[0].currentEnemies[0] == EmptyCheckEnemy)
        {
            enemy1 = false;
        }
        else
        {
            enemy1 = true;
            enemies1 = new(enemyTeams.ActiveEnemies[0].currentEnemies[0], enemyTeams.ActiveEnemiesLevel[0][0]);
            RectTransform spawn1 = spawnPositionEnemies1.GetComponent<RectTransform>();
            spawn1.sizeDelta = new Vector2 (enemies1.characters.transform.localScale.x, enemies1.characters.transform.localScale.y);
            Instantiate(enemies1.characters, spawnPositionEnemies1.transform.position, Quaternion.identity, spawnPositionEnemies1.transform);
            enemiesHUD1.transform.position = new Vector3(enemiesHUD1.transform.position.x,(spawn1.transform.position.y + spawn1.rect.height)/1.5f);
            spawnListEnemies.Add(spawnPositionEnemies1);
            enemiesHUDs.Add(enemiesHUD1);
        }
        if(enemyTeams.ActiveEnemies[0].currentEnemies[1] == EmptyCheckEnemy)
        {
            enemy2 = false;
        }
        else
        {
            enemy2 = true;
            enemies2 = new(enemyTeams.ActiveEnemies[0].currentEnemies[1], enemyTeams.ActiveEnemiesLevel[0][1]);
            RectTransform spawn2 = spawnPositionEnemies2.GetComponent<RectTransform>();
            spawn2.sizeDelta = new Vector2(enemies2.characters.transform.localScale.x, enemies2.characters.transform.localScale.y);
            Instantiate(enemies2.characters, spawnPositionEnemies2.transform.position, Quaternion.identity, spawnPositionEnemies2.transform);
            enemiesHUD2.transform.position = new Vector3(enemiesHUD2.transform.position.x, (spawn2.transform.position.y + spawn2.rect.height) / 1.5f);
            spawnListEnemies.Add(spawnPositionEnemies2);
            enemiesHUDs.Add(enemiesHUD2);
        }
        if (enemyTeams.ActiveEnemies[0].currentEnemies[2] == EmptyCheckEnemy)
        {
            enemy3 = false;
        }
        else
        {
            enemy3 = true;
            enemies3 = new(enemyTeams.ActiveEnemies[0].currentEnemies[2], enemyTeams.ActiveEnemiesLevel[0][2]);
            RectTransform spawn3 = spawnPositionEnemies3.GetComponent<RectTransform>();
            spawn3.sizeDelta = new Vector2(enemies3.characters.transform.localScale.x, enemies3.characters.transform.localScale.y);
            Instantiate(enemies3.characters, spawnPositionEnemies3.transform.position, Quaternion.identity, spawnPositionEnemies3.transform);
            enemiesHUD3.transform.position = new Vector3(enemiesHUD3.transform.position.x, (spawn3.transform.position.y + spawn3.rect.height) / 1.5f);
            spawnListEnemies.Add(spawnPositionEnemies3);
            enemiesHUDs.Add(enemiesHUD3);
        }

    }
    public void DestroyEnemies(int i)
    {
        Destroy(spawnListEnemies[i]);
        spawnListEnemies.RemoveAt(i);
        Destroy(enemiesHUDs[i]);
        enemiesHUDs.RemoveAt(i);
    }
    public void DestroyHeroes(int i)
    {
        //Destroy(spawnListHeroes[i]);
        var animator = spawnListHeroes[i].GetComponentInChildren<Animator>();
        animator.Play("DEATH");
        spawnListHeroes.RemoveAt(i);
        Destroy(playerHUDs[i]);
        playerHUDs.RemoveAt(i);
    }
    public void SetAnimation(int i, HeroBase.AnimType anim)
    {
        var animator = spawnListHeroes[i].GetComponentInChildren<Animator>();
        Debug.Log(animator);
        if(anim == HeroBase.AnimType.Warrior)
        {
            if (battleSystem.basic == true)
            {
                animator.Play("0_Attack_Normal");
            }
            else
            {
                animator.Play("1_Skill_Normal");
            }
        }
        else if (anim == HeroBase.AnimType.Archer)
        {
            if (battleSystem.basic == true)
            {
                animator.Play("0_Attack_Bow");
            }
            else
            {
                animator.Play("1_Skill_Bow");
            }
        }
        else if (anim == HeroBase.AnimType.Mage)
        {
            if (battleSystem.basic == true)
            {
                animator.Play("0_Attack_Magic");
            }
            else
            {
                animator.Play("MageSkill");
            }
        }

    }
    public void ResetAnimation(int i)
    {
        var animator = spawnListHeroes[i].GetComponentInChildren<Animator>();
        animator.Play("IDLE");
    }



}
