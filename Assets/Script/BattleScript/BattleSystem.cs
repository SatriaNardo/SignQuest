using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PlayerSkills, PlayerItems, EnemyMove, Busy, WaitingTurn, PickingEnemies, PickingAllies, signPower}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD platerHUD;
    //[SerializeField] TurnIndicator turnIndicator;
    [SerializeField] Turn turnOrderUI;
    [SerializeField] BattleDialogueBox dialogueBox;
    [SerializeField] Button basicAttack;
    [SerializeField] Button skill;
    [SerializeField] Button items;
    [SerializeField] Button retreat;
    [SerializeField] Button pickEnemies1;
    [SerializeField] Button pickEnemies2;
    [SerializeField] Button pickEnemies3;
    [SerializeField] Button pickHero1;
    [SerializeField] Button pickHero2;
    [SerializeField] Button pickHero3;
    [SerializeField] GameObject skillsList;
    [SerializeField] GameObject itemsList;
    [SerializeField] GameObject actionList;
    [SerializeField] GameObject indicatorText;
    [SerializeField] GameObject firstTextPlace;
    [SerializeField] GameObject secondTextPlace;
    [SerializeField] GameObject thirdTextPlace;
    [SerializeField] RectTransform scrollLocation;
    [SerializeField] Button retreatButton;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] TextMeshProUGUI firstText;
    [SerializeField] TextMeshProUGUI secondText;
    [SerializeField] TextMeshProUGUI thirdText;
    [SerializeField] GameObject firstPanelSign;
    [SerializeField] GameObject secondPanelSign;
    [SerializeField] GameObject thirdPanelSign;
    public List<RectTransform> turnPanels = new List<RectTransform>();
    public List<Button> highlightEnemies = new List<Button>();
    public List<Button> highlightHeroes = new List<Button>();
    public List<LetterImage> letterImages = new List<LetterImage>();
    public List<Heroes> heroes = new List<Heroes>();
    public List<Enemies> enemies = new List<Enemies>();
    List<Heroes> readyHeroes = new List<Heroes>();
    List<Enemies> readyEnemies = new List<Enemies>();
    public Heroes curHeroes;
    public Enemies curEnemies;
    public float DecrementPerTick = 10f;
    private float tickInterval = 0f;
    private float nextTickTime = 0f;
    public bool selecting;
    public BattleState state;
    int currentAction = -1;
    int currentEnemies;
    int currentAlly;
    int totalEnemies;
    int kill;
    int dead;
    int attackMoves;
    int cury;
    int currentHeroForAnim;
    public bool frontlineCheck = false;
    public bool clicking = false;
    public bool typing = false;
    public bool basic = false;
    public bool picking = false;
    public bool healing = false;
    bool acting = false;
    EnemyManagers enemyTeams = EnemyManagers.Instance;
    UDPReceive signCode = UDPReceive.Instance;
    public Image firstImage, secondImage, thirdImage;
    [System.Serializable]
    public class LetterImage
    {
        public char letter;
        public Sprite image;
    }
    
    private Dictionary<char, Sprite> letterToSprite;

    private void Start()
    {
        StartCoroutine(SetupBattle());
        totalEnemies = highlightEnemies.Count;
        if (playerUnit.hero1 == true)
        {
            heroes.Add(playerUnit.heroes1);
        }
        if (playerUnit.hero2 == true)
        {
            heroes.Add(playerUnit.heroes2);
        }
        if (playerUnit.hero3 == true)
        {
            heroes.Add(playerUnit.heroes3);
        }
        if (playerUnit.enemy1 == true)
        {
            enemies.Add(playerUnit.enemies1);
        }
        if (playerUnit.enemy2 == true)
        {
            enemies.Add(playerUnit.enemies2);
        }
        if (playerUnit.enemy3 == true)
        {
            enemies.Add(playerUnit.enemies3);
        }
        List<object> allCombatants = new List<object>();
        allCombatants.AddRange(heroes);
        allCombatants.AddRange(enemies);
        // Initialize the turn order UI
        turnOrderUI.InitializeTurnOrderUI(allCombatants);
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        if (playerUnit.hero1)
        {
            //turnIndicator.SetTurnHero1(playerUnit.heroes1);
            platerHUD.SetDataHeroes1(playerUnit.heroes1);
            //turnPanels.Add(turnIndicator.Unit1Panels);
            highlightHeroes.Add(pickHero1);

        }
        else
        {
            //turnIndicator.Unit1Turn.SetActive(false);
            playerUnit.playerHUD1.SetActive(false);
        }
        if (playerUnit.hero2)
        {
            //turnIndicator.SetTurnHero2(playerUnit.heroes2);
            platerHUD.SetDataHeroes2(playerUnit.heroes2);
            //turnPanels.Add(turnIndicator.Unit2Panels);
            highlightHeroes.Add(pickHero2);
        }
        else
        {
            playerUnit.playerHUD2.SetActive(false);
            //turnIndicator.Unit2Turn.SetActive(false);
        }
        if (playerUnit.hero3)
        {
            //turnIndicator.SetTurnHero3(playerUnit.heroes3);
            platerHUD.SetDataHeroes3(playerUnit.heroes3);
            //turnPanels.Add(turnIndicator.Unit3Panels);
            highlightHeroes.Add(pickHero3);
        }
        else
        {
            //turnIndicator.Unit3Turn.SetActive(false);
            playerUnit.playerHUD3.SetActive(false);
        }


        if (playerUnit.enemy1)
        {
            
            //turnIndicator.SetTurnEnemies1(playerUnit.enemies1);
            platerHUD.SetDataEnemies1(playerUnit.enemies1);
            //turnPanels.Add(turnIndicator.Unit4Panels);
            highlightEnemies.Add(pickEnemies1);
        }
        else
        {
            //turnIndicator.Unit4Turn.SetActive(false);
            playerUnit.enemiesHUD1.SetActive(false);
        }
        if (playerUnit.enemy2)
        {
            
            //turnIndicator.SetTurnEnemies2(playerUnit.enemies2);
            platerHUD.SetDataEnemies2(playerUnit.enemies2);
            //turnPanels.Add(turnIndicator.Unit5Panels);
            highlightEnemies.Add(pickEnemies2);
        }
        else
        {
            //turnIndicator.Unit5Turn.SetActive(false);
            playerUnit.enemiesHUD2.SetActive(false);
        }
        if (playerUnit.enemy3)
        {
            
            //turnIndicator.SetTurnEnemies3(playerUnit.enemies3);
            platerHUD.SetDataEnemies3(playerUnit.enemies3);
            //turnPanels.Add(turnIndicator.Unit6Panels);
            highlightEnemies.Add(pickEnemies3);
        }
        else
        {
            //turnIndicator.Unit6Turn.SetActive(false);
            playerUnit.enemiesHUD3.SetActive(false);
        }

        
        yield return (dialogueBox.TypeDialog("BATTLE COMMENCED!"));
        state = BattleState.WaitingTurn;
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void TurnWAA()
    {
        if(!acting)
        {
            currentHeroForAnim = -1;
        }
        // Create a combined list of heroes and enemies for sorting
        List<object> allCombatants = new List<object>();
        allCombatants.AddRange(heroes);  // Add all heroes
        allCombatants.AddRange(enemies); // Add all enemies
        
        // Sort by ActionValue in ascending order
        allCombatants.Sort((a, b) =>
        {
            float aValue = (a is Heroes h) ? h.ActionValue : ((Enemies)a).ActionValue;
            float bValue = (b is Heroes hero) ? hero.ActionValue : ((Enemies)b).ActionValue;
            return aValue.CompareTo(bValue);
        });
        
        foreach (Heroes heroes in heroes)
        {
            if (state == BattleState.WaitingTurn)
            {
                ++currentHeroForAnim;
                dialogueBox.SetDialog("Waiting For Turn");
                if(heroes.ActionValue != 0f)
                {
                    heroes.ActionValue -= DecrementPerTick;
                }
                turnOrderUI.UpdateTurnOrderUI(allCombatants);
                //turnIndicator.Hero1UpdateTurn();
                //turnIndicator.Hero2UpdateTurn();
                if (heroes.ActionValue <= 0)
                {
                    dialogueBox.SetDialog("Its Your Turn, Choose your action");
                    state = BattleState.PlayerAction;
                    curHeroes = heroes;
                    dialogueBox.ClearMoves();
                    dialogueBox.ClearItem();
                    dialogueBox.SetMoves(heroes.Move);
                    dialogueBox.SetUsableItems();
                    heroes.ActionValue = 10000f / heroes.bases.speed;
                    acting = true;
                }
                
            }
            else if(state == BattleState.PlayerAction)
            {
                basicAttack.interactable = true;
                skill.interactable = true;
                items.interactable = true;
                retreat.interactable = true;
                HandleActionSelector();
                break;
            }
            
        
        }
        foreach (Enemies enemies in enemies)
        {
            if(state == BattleState.WaitingTurn)
            {
                if(enemies.ActionValue != 0f)
                {
                    enemies.ActionValue -= DecrementPerTick;
                }
                turnOrderUI.UpdateTurnOrderUI(allCombatants);
                //turnIndicator.Enemies1UpdateTurn();
                //turnIndicator.Enemies2UpdateTurn();
                //turnIndicator.Enemies3UpdateTurn();
                if (enemies.ActionValue <= 0)
                {
                    state = BattleState.EnemyMove;
                    curEnemies = enemies;
                    StartCoroutine(EnemyMove(enemies));
                    enemies.ActionValue = 10000f / enemies.bases.speed;
                }
            }
            
        }
         
    }
    void PlayerAction()
    {
        basicAttack.interactable = true;
        skill.interactable = true;
        items.interactable = true;
        retreat.interactable = true;
    }
    public IEnumerator PerformPlayerMove(Heroes hero, int i, int moves, int targetAllyIndex = -1)
    {
        string damageNumber = "0";
        string healingNumber = "0";
        bool isFainted = false;
        int curi = i;
        Debug.Log("Ini curi " + curi);
        if(totalEnemies == 3)
        {
            if (kill == 1)
            {
                if (i == 2)
                {
                    --i;
                }
            }
            else if (kill == 2)
            {
                i = 0;
            }
        }
        else if(totalEnemies == 2)
        {
            Debug.Log("WaaaAagagagaaa");
            if (kill == 1)
            {
                i = 0;
            }
        }
        cury = i;
        Debug.Log("Ini cury " + i);
        var move = hero.Move[moves];
        state = BattleState.Busy;
        if(basic == true)
        {
            move = hero.Move[0];
            yield return dialogueBox.TypeDialog($"{hero.bases.Name} launching Basic Attack");
        }
        else
        {
            move = hero.Move[moves];
            yield return dialogueBox.TypeDialog($"{hero.bases.Name} used {move.Base.name}");
        }
        if (move.Base.AttacksType == MoveBase.AttackType.Healing)
        {
            healing = true;
            yield return dialogueBox.TypeDialog($"{hero.bases.Name} is casting a healing spell!");
        }

        yield return new WaitForSeconds(0.5f);
        // Enter slow motion for 6 seconds and allow key input
        float originalTimeScale = Time.timeScale;
        playerUnit.SetAnimation(currentHeroForAnim, hero.bases.heroType);
        Time.timeScale = 0.2f; // Slow motion
        yield return StartCoroutine(HandleBuffInput());
        Time.timeScale = originalTimeScale; // Restore time scale

        // Calculate damage multiplier based on key inputs
        float damageMultiplier = CalculateBuffMultiplier();
        //Debug.Log($"Damage multiplier: {damageMultiplier}");
        yield return new WaitForSeconds(0.5f);

        if (healing == true)
        {
            float healAmount = move.Base.BasePower; // you can add multipliers, buffs, randomness etc.
            Heroes targetHero = (targetAllyIndex >= 0) ? heroes[targetAllyIndex] : hero; // default to self if index invalid
            targetHero.Heal(healAmount);
            healingNumber = healAmount.ToString();
            Debug.Log(targetAllyIndex);
            DisplayHealing(healingNumber, targetAllyIndex >= 0 ? targetAllyIndex : i);
            yield return new WaitForSeconds(0.5f);
            yield return platerHUD.UpdateHpHeroes(targetAllyIndex, heroes[targetAllyIndex]);

        }
        else if(healing == false)
        {
            isFainted = enemies[i].TakeDamage(move, hero, damageMultiplier);
            Debug.Log(hero.bases.heroType);
            damageNumber = enemies[i].TakeDamageNumbers(move, hero, damageMultiplier);
            DisplayDamage(damageNumber, i);
            yield return new WaitForSeconds(0.5f);
            yield return platerHUD.UpdateHpEnemies(i, enemies[i]);
        }
        yield return new WaitForSeconds(1f);
        playerUnit.ResetAnimation(currentHeroForAnim);
        
        if (isFainted)
        {
            if(i == 0)
            {
                frontlineCheck = true;
            }
            DropLoot(enemies[i].bases);
            enemies.RemoveAt(i);
            playerUnit.DestroyEnemies(cury);
            platerHUD.DestroyBarsEnemies(i);
            highlightEnemies[i] = null;
            highlightEnemies.RemoveAt(i);
            ++kill;
            if(enemies.Count == 0)
            {
                enemyTeams.ActiveEnemies[0].battleWon = true;
                enemyTeams.DeregisterEnemy();
                retreat.onClick.Invoke();
            }
            state = BattleState.WaitingTurn;
            acting = false;
            TurnWAA();
        }
        else
        {
            state = BattleState.WaitingTurn;
            acting = false;
            TurnWAA();
        }
        typing = false;
        basic = false;
        healing = false;
        
        
    }
    private IEnumerator HandleBuffInput()
    {
        letterToSprite = letterImages.ToDictionary(x => x.letter, x => x.image);
        //string alphabet = "abcdefghiklmnopqrstuvwxy";
        int learning = PlayerPrefs.GetInt("IsToggleOn");
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        string[] keySequence = alphabet.ToCharArray().OrderBy(c => Random.value).Take(3).Select(c => c.ToString()).ToArray();
        string firstWord = keySequence[0];
        string secondWord = keySequence[1];
        string thirdWord = keySequence[2];
        int correctKeys = 0;

        firstText.text = firstWord;
        secondText.text = secondWord;
        thirdText.text = thirdWord;

        firstImage.sprite = letterToSprite[firstWord[0]];
        secondImage.sprite = letterToSprite[secondWord[0]];
        thirdImage.sprite = letterToSprite[thirdWord[0]];

        indicatorText.SetActive(true);
        firstTextPlace.SetActive(true);
        secondTextPlace.SetActive(true);
        thirdTextPlace.SetActive(true);

        if(learning == 0)
        {
            firstPanelSign.SetActive(false);
            secondPanelSign.SetActive(false);
            thirdPanelSign.SetActive(false);
        }
        else if (learning == 1)
        {
            firstPanelSign.SetActive(true);
            secondPanelSign.SetActive(true);
            thirdPanelSign.SetActive(true);
        }
        for (int index = 0; index < keySequence.Length; index++)
        {
            bool keyMatched = false;
            float timer = 2f; // Allow 2 seconds for each input during slow motion

            while (timer > 0)
            {
                timer -= Time.unscaledDeltaTime; // Use unscaled delta time during slow motion
                if (signCode.data.ToLower() == keySequence[index]) 
                {
                    keyMatched = true;
                    if(correctKeys == 0)
                    {
                        firstTextPlace.SetActive(false);
                    }
                    if (correctKeys == 1)
                    {
                        secondTextPlace.SetActive(false);
                    }
                    if (correctKeys == 2)
                    {
                        thirdTextPlace.SetActive(false);
                    }
                    correctKeys++;
                    break;
                }

                yield return null;
            }

            if (!keyMatched)
            {
               
            }
        }
        indicatorText.SetActive(false);
        // Store the number of correct keys
        PlayerPrefs.SetInt("CorrectKeys", correctKeys); // Temporary storage for calculation
    }

    // Method to calculate the buff multiplier
    private float CalculateBuffMultiplier()
    {
        int correctKeys = PlayerPrefs.GetInt("CorrectKeys", 0);
        string buffText;
        float multiplier;

        switch (correctKeys)
        {
            case 0:
                buffText = "BAD";
                multiplier = 0.5f;
                break;
            case 1:
                buffText = "DECENT";
                multiplier = 0.8f;
                break;
            case 2:
                buffText = "OKAY";
                multiplier = 1.0f;
                break;
            case 3:
                buffText = "GREAT";
                multiplier = 1.5f;
                break;
            default:
                buffText = "PERFECT";
                multiplier = 1.0f;
                break;
        }

        // Only display buff text if targeting enemies
        if (state == BattleState.PickingEnemies || state == BattleState.Busy)
        {
            if (cury >= 0 && cury < enemies.Count)
            {
                DisplayDamage(buffText, cury);
            }
        }

        return multiplier;
    }

    public IEnumerator EnemyMove(Enemies enemies)
    {
        state = BattleState.EnemyMove;

        var move = enemies.GetRandomMoves();
        int pickedHeroes = Random.Range(0, heroes.Count);
        int pickedHeroesStay = pickedHeroes;
        if (dead == 1)
        {
            if (pickedHeroes == 2)
            {
                --pickedHeroes;
            }
        }
        else if (dead == 2)
        {
            pickedHeroes = 0;
        }
        dialogueBox.SetDialog($"{enemies.bases.Name} launching {move.Base.name}");
        yield return new WaitForSeconds(0.5f);
        bool isFainted = heroes[pickedHeroes].TakeDamage(move, enemies);
        yield return platerHUD.UpdateHpHeroes(pickedHeroes, heroes[pickedHeroes]);
        if (isFainted)
        {
            heroes.RemoveAt(pickedHeroes);
            playerUnit.DestroyHeroes(pickedHeroesStay);
            platerHUD.DestroyBarsHeroes(pickedHeroes);
            ++dead;
            if (heroes.Count == 0)
            {
                enemyTeams.DeregisterEnemy();
                MMSceneLoadingManager.LoadScene("GrassLand");
            }
            state = BattleState.WaitingTurn;
            TurnWAA();
        }
        else
        {
            state = BattleState.WaitingTurn;
            TurnWAA();
        }
        for (int y = 0; y < highlightEnemies.Count; y++)
        {
            if (highlightEnemies[y] != null)
            {
                highlightEnemies[y].interactable = false;
            }
        }
        typing = false;
    }
    public void BasicAttack()
    {
        typing = true;
        basic = true;
        basicAttack.interactable = false;
        skill.interactable = false;
        items.interactable = false;
        retreat.interactable = false;
        state = BattleState.PickingEnemies;
        attackMoves = 0;
    }
    public void ChangeStatesSkills()
    {
        state = BattleState.PlayerSkills;
    }
    public void ChangeStatesItems()
    {
        state = BattleState.PlayerItems;
    }
    public void ChangeStatesAction()
    {
        state = BattleState.PlayerAction;
    }
    public void attackEnemy1()
    {
        if(state == BattleState.PickingEnemies)
        {
            state = BattleState.Busy;
            StartCoroutine(PerformPlayerMove(curHeroes, 0, attackMoves));
        }
    }
    public void attackEnemy2()
    {
        if(state == BattleState.PickingEnemies)
        {
            state = BattleState.Busy;
            if (frontlineCheck == true)
            {
                StartCoroutine(PerformPlayerMove(curHeroes, 0, attackMoves));
            }
            else
            {
                StartCoroutine(PerformPlayerMove(curHeroes, 1, attackMoves));
            }
            
        }
    }
    public void attackEnemy3()
    {
        if(state == BattleState.PickingEnemies)
        {
            state = BattleState.Busy;
            StartCoroutine(PerformPlayerMove(curHeroes, 2, attackMoves));
        }
    }
    private void Update()
    {
        if (state == BattleState.WaitingTurn)
        {

            if (Time.time >= nextTickTime)
            {
                TurnWAA();
                nextTickTime = Time.time + tickInterval;
            }
        }
        else if(state == BattleState.PlayerAction)
        {
            TurnWAA();
        }
        else if (state == BattleState.PlayerSkills)
        {
            HandleSkillSelector();
        }
        else if (state == BattleState.PlayerItems)
        {
            HandleItemSelector();
        }
        else if (state == BattleState.PickingEnemies)
        {
            for (int y = 0; y < highlightEnemies.Count; y++)
            {
                if (highlightEnemies[y] != null)
                {
                    highlightEnemies[y].interactable = true;
                }
            }
            dialogueBox.SetDialog("Pick Enemies");
            
            HandleEnemiesSelector();
            skillsList.SetActive(false);
            itemsList.SetActive(false);
            actionList.SetActive(true);
        }
        else if (state == BattleState.PickingAllies)
        {
            for (int y = 0; y < highlightHeroes.Count; y++)
            {
                if (highlightHeroes[y] != null)
                {
                    highlightHeroes[y].interactable = true;
                }
            }
            dialogueBox.SetDialog("Pick Allies");

            HandleAlliesSelector();
            skillsList.SetActive(false);
            itemsList.SetActive(false);
            actionList.SetActive(true);
        }
    }
    void HandleActionSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selecting = true;
            if(currentAction < 3)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selecting = true;
            if (currentAction > 0)
                --currentAction;
        }
        dialogueBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentAction == 0)
            {
                clicking = true;
                basic = true;
                state = BattleState.PickingEnemies;
                basicAttack.interactable = false;
                skill.interactable = false;
                items.interactable = false;
                retreat.interactable = false;
                attackMoves = 0;
                //StartCoroutine(PerformPlayerMove(heroes));
                
            }
            else if(currentAction == 1)
            {
                skillsList.SetActive(true);
                actionList.SetActive(false);
                state = BattleState.PlayerSkills;
                currentAction = 0;
                scrollLocation.transform.localPosition = new Vector3(scrollLocation.transform.localPosition.x, -1000, scrollLocation.transform.localPosition.z);
            }
            else if(currentAction == 2)
            {
                itemsList.SetActive(true);
                actionList.SetActive(false);
                state = BattleState.PlayerItems;
                currentAction = 0;
            }
            else if(currentAction == 3)
            {
                enemyTeams.DeregisterEnemy();
                retreat.onClick.Invoke();
            }
        }
    }
    void HandleSkillSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selecting = true;
            if (currentAction < dialogueBox.moveSelect.Count - 1)
            {
                scrollLocation.transform.localPosition = new Vector3(
                    scrollLocation.transform.localPosition.x,
                    scrollLocation.transform.localPosition.y + 30.43f,
                    scrollLocation.transform.localPosition.z
                );
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selecting = true;
            if (currentAction > 0)
            {
                scrollLocation.transform.localPosition = new Vector3(
                    scrollLocation.transform.localPosition.x,
                    scrollLocation.transform.localPosition.y - 30.43f,
                    scrollLocation.transform.localPosition.z
                );
                --currentAction;
            }
        }

        // Update UI preview
        dialogueBox.UpdateMoveSelection(currentAction, curHeroes.Move[currentAction + 1]);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            skillsList.SetActive(false);
            actionList.SetActive(true);
            itemsList.SetActive(false);
            state = BattleState.PlayerAction;
            currentAction = 1;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get selected move
            var selectedMove = curHeroes.Move[currentAction + 1];

            skillsList.SetActive(true);
            actionList.SetActive(false);
            attackMoves = currentAction + 1;

            // Check move type and update state accordingly
            if (selectedMove.Base.AttacksType == MoveBase.AttackType.Healing)
            {
                state = BattleState.PickingAllies; // You'll need to handle this new state
            }
            else
            {
                state = BattleState.PickingEnemies;
            }
        }
    }
    void HandleItemSelector()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selecting = true;
            if (currentAction < dialogueBox.itemSelect.Count - 1)
            {
                scrollLocation.transform.localPosition = new Vector3(
                    scrollLocation.transform.localPosition.x,
                    scrollLocation.transform.localPosition.y + 30.43f,
                    scrollLocation.transform.localPosition.z
                );
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selecting = true;
            if (currentAction > 0)
            {
                scrollLocation.transform.localPosition = new Vector3(
                    scrollLocation.transform.localPosition.x,
                    scrollLocation.transform.localPosition.y - 30.43f,
                    scrollLocation.transform.localPosition.z
                );
                --currentAction;
            }
        }

        // Update UI preview
        Inventory targetInventory = Inventory.FindInventory("KoalaMainInventory", "Player1");
        InventoryItem[] allItems = targetInventory.Content;
        List<InventoryItem> consumableItems = new List<InventoryItem>();
        foreach (InventoryItem item in allItems)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemName) || item.Quantity <= 0)
                continue;

            if (!item.Usable)
                continue;

            if (item.Usable)
            {
                consumableItems.Add(item);
            }
        }
        if(consumableItems.Count > 0)
        {
            dialogueBox.UpdateItemSelection(currentAction, consumableItems[currentAction]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skillsList.SetActive(false);
            actionList.SetActive(true);
            itemsList.SetActive(false);
            state = BattleState.PlayerAction;
            currentAction = 1;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get selected move
            var selectedItem = consumableItems[0];

            itemsList.SetActive(true);
            actionList.SetActive(false);
            attackMoves = currentAction + 1;

            // Check move type and update state accordingly
            if (selectedItem.ItemClass == ItemClasses.Healing)
            {
                Debug.Log("Using Potion");
                state = BattleState.PickingAllies; // You'll need to handle this new state
                healing = true;
            }
            else if (selectedItem.ItemClass == ItemClasses.Spell)
            {
                state = BattleState.PickingEnemies;
            }
        }
    }
    void HandleEnemiesSelector()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentEnemies < enemies.Count - 1)
            {
                ++currentEnemies;
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentEnemies > 0)
            {
                --currentEnemies;
            }
        }
        UpdateEnemiesSelection(currentEnemies);
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = BattleState.Busy;
            StartCoroutine(PerformPlayerMove(curHeroes, currentEnemies, attackMoves));
            currentEnemies = 0; 
            for (int y = 0; y < highlightEnemies.Count; y++)
            {
                if (highlightEnemies[y] != null)
                {
                    highlightEnemies[y].interactable = false;
                }
            }
            currentAction = -1;
            dialogueBox.ClearMoves();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int y = 0; y < highlightEnemies.Count; y++)
            {
                if (highlightEnemies[y] != null)
                {
                    highlightEnemies[y].interactable = false;
                }
            }
            clicking = false;
            basic = false;
            state = BattleState.PlayerAction;
            currentAction = -1;
            dialogueBox.SetDialog("Choose your action");
        }
    }
    void HandleAlliesSelector()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAlly > 0)
            {
                --currentAlly;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAlly < heroes.Count - 1)
            {
                ++currentAlly;
            }
        }

        UpdateHeroesSelection(currentAlly); // You'll implement this like UpdateEnemiesSelection

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = BattleState.Busy;
            StartCoroutine(PerformPlayerMove(curHeroes, -1, attackMoves, currentAlly));
            // Send ally index instead of enemy
            currentAlly = 0;

            for (int y = 0; y < highlightHeroes.Count; y++)
            {
                if (highlightHeroes[y] != null)
                {
                    highlightHeroes[y].interactable = false;
                }
            }

            currentAction = -1;
            dialogueBox.ClearMoves();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int y = 0; y < highlightHeroes.Count; y++)
            {
                if (highlightHeroes[y] != null)
                {
                    highlightHeroes[y].interactable = false;
                }
            }

            clicking = false;
            basic = false;
            state = BattleState.PlayerAction;
            currentAction = -1;
            dialogueBox.SetDialog("Choose your action");
        }
    }

    void UpdateEnemiesSelection(int selectedEnemies)
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if(i == selectedEnemies)
            {

                ColorBlock colorBlock = highlightEnemies[i].colors;

                // Modify the alpha of the normal color
                Color normalColor = colorBlock.normalColor;
                normalColor.a = 1f; // Adjust alpha to your desired value (1f for fully opaque)
                colorBlock.normalColor = normalColor;

                // Reassign the modified ColorBlock back to the component
                highlightEnemies[i].colors = colorBlock;
            }
            else
            {
                ColorBlock colorBlock = highlightEnemies[i].colors;

                // Modify the alpha of the normal color
                Color normalColor = colorBlock.normalColor;
                normalColor.a = 0f; // Adjust alpha to your desired value (1f for fully opaque)
                colorBlock.normalColor = normalColor;

                // Reassign the modified ColorBlock back to the component
                highlightEnemies[i].colors = colorBlock;
            }
        }
    }
    void UpdateHeroesSelection(int selectedHeroes)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            if (i == selectedHeroes)
            {

                ColorBlock colorBlock = highlightHeroes[i].colors;

                // Modify the alpha of the normal color
                Color normalColor = colorBlock.normalColor;
                normalColor.a = 1f; // Adjust alpha to your desired value (1f for fully opaque)
                colorBlock.normalColor = normalColor;

                // Reassign the modified ColorBlock back to the component
                highlightHeroes[i].colors = colorBlock;
            }
            else
            {
                ColorBlock colorBlock = highlightHeroes[i].colors;

                // Modify the alpha of the normal color
                Color normalColor = colorBlock.normalColor;
                normalColor.a = 0f; // Adjust alpha to your desired value (1f for fully opaque)
                colorBlock.normalColor = normalColor;

                // Reassign the modified ColorBlock back to the component
                highlightHeroes[i].colors = colorBlock;
            }
        }
    }


    public void DisplayDamage(string damage, int i)
    {
        if (damageTextPrefab != null)
        {
            // Instantiate the damage text prefab at the enemy's position
            var damageTextInstance = Instantiate(damageTextPrefab, highlightEnemies[i].transform.position, Quaternion.identity, transform.Find("UICanvas"));

            // Set the damage number
            var textComponent = damageTextInstance.GetComponent<TextMeshProUGUI>(); // Or Text
            if (textComponent != null)
            {
                textComponent.text = damage.ToString();
            }
            var animator = damageTextInstance.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("DamageAnim"); // Replace with the actual animation name
            }

            // Optionally, destroy the prefab after a few seconds
            StartCoroutine(FadeOutAndDestroy(damageTextInstance, 0.4f));
        }
    }
    public void DisplayHealing(string damage, int i)
    {
        if (damageTextPrefab != null)
        {
            // Instantiate the damage text prefab at the enemy's position
            var healingTextInstance = Instantiate(damageTextPrefab, highlightHeroes[i].transform.position, Quaternion.identity, transform.Find("UICanvas"));

            // Set the damage number
            var textComponent = healingTextInstance.GetComponent<TextMeshProUGUI>(); // Or Text
            if (textComponent != null)
            {
                textComponent.text = damage.ToString();
            }
            var animator = healingTextInstance.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("DamageAnim"); // Replace with the actual animation name
            }

            // Optionally, destroy the prefab after a few seconds
            StartCoroutine(FadeOutAndDestroy(healingTextInstance, 0.4f));
        }
    }
    private IEnumerator FadeOutAndDestroy(GameObject damageTextInstance, float duration)
    {
        var textComponent = damageTextInstance.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            // Get the initial color
            Color color = textComponent.color;
            yield return new WaitForSeconds(1f);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                // Reduce the alpha value over time
                color.a = Mathf.Lerp(1f, 0f, t);
                textComponent.color = color;

                yield return null;
            }

            // Destroy the object after fading out
            Destroy(damageTextInstance);
        }
    }
    private void DropLoot(EnemiesBase enemyBase)
    {
        Inventory mainInventory = Inventory.FindInventory("KoalaMainInventory", "Player1");

        if (mainInventory == null)
        {
            Debug.LogWarning("Main inventory not found!");
            return;
        }

        foreach (var drop in enemyBase.Drops)
        {
            float roll = Random.value;
            if (roll <= drop.dropChance)
            {
                mainInventory.AddItem(drop.item, drop.item.Quantity);
                Debug.Log($"Dropped: {drop.item.ItemName}");
            }
        }
    }
}
