using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unit1AV;
    [SerializeField] TextMeshProUGUI unit2AV;
    [SerializeField] TextMeshProUGUI unit3AV;
    [SerializeField] TextMeshProUGUI unit4AV;
    [SerializeField] TextMeshProUGUI unit5AV;
    [SerializeField] TextMeshProUGUI unit6AV;
    [SerializeField] public RectTransform Unit1Panels;
    [SerializeField] public RectTransform Unit2Panels;
    [SerializeField] public RectTransform Unit3Panels;
    [SerializeField] public RectTransform Unit4Panels;
    [SerializeField] public RectTransform Unit5Panels;
    [SerializeField] public RectTransform Unit6Panels;
    [SerializeField] public GameObject Unit1Turn;
    [SerializeField] public GameObject Unit2Turn;
    [SerializeField] public GameObject Unit3Turn;
    [SerializeField] public GameObject Unit4Turn;
    [SerializeField] public GameObject Unit5Turn;
    [SerializeField] public GameObject Unit6Turn;
    [SerializeField] GameObject faceTurn1;
    [SerializeField] GameObject faceTurn2;
    [SerializeField] GameObject faceTurn3;
    [SerializeField] GameObject faceTurn4;
    [SerializeField] GameObject faceTurn5;
    [SerializeField] GameObject faceTurn6;

    Heroes _heroes1;
    Heroes _heroes2;
    Heroes _heroes3;
    Enemies _enemies1;
    Enemies _enemies2;
    Enemies _enemies3;
    public void SetTurnHero1(Heroes heroes)
    {
        _heroes1 = heroes;
        unit1AV.text = "" + heroes.ActionValue;
        Instantiate(heroes.Faces, faceTurn1.transform);
    }
    public void SetTurnHero2(Heroes heroes)
    {
        _heroes2 = heroes;
        unit2AV.text = "" + heroes.ActionValue;
        Instantiate(heroes.Faces, faceTurn2.transform);
    }
    public void SetTurnHero3(Heroes heroes)
    {
        _heroes3 = heroes;
        unit3AV.text = "" + heroes.ActionValue;
        Instantiate(heroes.Faces, faceTurn3.transform);
    }
    public void SetTurnEnemies1(Enemies enemies)
    {
        _enemies1 = enemies;
        unit4AV.text = "" + enemies.ActionValue;
        Instantiate(enemies.Faces, faceTurn4.transform);
    }
    public void SetTurnEnemies2(Enemies enemies)
    {
        _enemies2 = enemies;
        unit5AV.text = "" + enemies.ActionValue;
        Instantiate(enemies.Faces, faceTurn5.transform);
    }
    public void SetTurnEnemies3(Enemies enemies)
    {
        _enemies3 = enemies;
        unit6AV.text = "" + enemies.ActionValue;
        Instantiate(enemies.Faces, faceTurn6.transform);
    }
    public void Hero1UpdateTurn()
    {
        unit1AV.text = "" + _heroes1.ActionValue; 
    }
    public void Hero2UpdateTurn()
    {
        unit2AV.text = "" + _heroes2.ActionValue;
    }
    public void Hero3UpdateTurn()
    {
        unit3AV.text = "" + _heroes3.ActionValue;
    }
    public void Enemies1UpdateTurn()
    {
        unit4AV.text = "" + _enemies1.ActionValue;
    }
    public void Enemies2UpdateTurn()
    {
        unit5AV.text = "" + _enemies1.ActionValue;
    }
    public void Enemies3UpdateTurn()
    {
        unit6AV.text = "" + _enemies1.ActionValue;
    }

}
