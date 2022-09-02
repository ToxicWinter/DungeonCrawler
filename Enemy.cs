using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterClass
{
    private GameManager gameManager;
    [SerializeField] GameObject player, lootBag; 
    public GameObject LootBag { get => lootBag; set => lootBag = value; }
    [SerializeField] Sprite corpse;
    [SerializeField] float lootChance = 100;
    private int EXP;
    private float hasLootRND;
    [SerializeField] int addItemLevel;
   

    protected override void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        MyTarget = player;
        base.Start();
        addItemLevel = addItemLevel + ((Strength.Value + Dexterity.Value + Constitution.Value)/3);
    }

    private void droppedLoot()
    {
        hasLootRND = Mathf.Round((Random.value * 100));
        lootChance += (player.GetComponent<PlayerController>().Dexterity.Value) * 0.5f;
        if (hasLootRND > lootChance)
        {
            LootBag = null;
        }
        else
        {
            LootBag = Instantiate(LootBag, this.GetComponent<Transform>());
            LootBag.GetComponent<LootBag>().ItemStats = addItemLevel;
            LootBag.GetComponent<Transform>().parent = null;
        }
    }

    private void Update() 
    {
        if (this.tag == "Enemy")
        {
            if(playerIsInRange() && (gameManager.GamePaused == false))
                StartCoroutine(WeaponAttack());
        }
    }

    
    private bool playerIsInRange()
    {
        if (this.gameObject.GetComponent<BoxCollider2D>().IsTouching(MyTarget.GetComponent<BoxCollider2D>()) && MyTarget.GetComponent<CharacterClass>().currentHP > 0)
            return true;
        else
            return false;
    }
    public int calculateEXPValue()
    {
        EXP = Strength.Value*10 + Constitution.Value*10 + Dexterity.Value*10;
        return EXP;
    }

    public void EnemyDied(){
        droppedLoot();
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.tag = "Untagged";
        this.GetComponent<SpriteRenderer>().sprite = corpse;
        this.gameObject.layer = 0;
       
    }
}
