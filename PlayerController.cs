using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterClass
{
    public Transform movePoint;
    private int lastMaxHP = 0;
    public bool isDirtyHP {get; set;}
    [SerializeField] GameManager gameManager;
    private int currentLevel;
    private int[] toLevelUp = new int[99]; 
    private int currentEXP;
    [SerializeField] BarScript experienceBar;
    public int StatPoints {get; set;}
    public bool LevelDirty {get; set;}
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject levelUpText;

    protected override void Start()
    { 
        movePoint.parent = null;
        isDirtyHP = true;
        LevelSetup();
        LevelDirty = false;
        CurrentLevel = 1;
        experienceBar.MaxValue = toLevelUp[CurrentLevel];
        calculateExperience(0);
        base.Start();
    }
     

    void Update()
    {
        if(LevelDirty)
        {
            levelUpText.SetActive(true);
        }
        else
        {
            levelUpText.SetActive(false);
        }
        calculateMaxHP();
        playerMovement();        
        //if (Input.GetKeyDown(KeyCode.Space))
       // {
            if(MyTarget != null)
                if (MyTarget.tag == "Enemy")
                    StartCoroutine(WeaponAttack());
       // }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(MyTarget != null)
            {
                if(this.gameObject.GetComponent<BoxCollider2D>().IsTouching(MyTarget.GetComponent<BoxCollider2D>()))
                {
                    if (MyTarget.tag == "Lootable")
                    {
                        playerLooting();
                    }
                    else if (MyTarget.tag == "Interactable")
                    {
                        MyTarget.GetComponent<Interactable>().Interact();
                    } 
                } 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "WalkOver")
        {
            col.gameObject.GetComponent<Interactable>().Interact();
        }
    }

    void playerLooting()
    {
        if(!inventory.IsFull())
        {
            inventory.AddItem(MyTarget.GetComponent<LootBag>().LootItem);
            Destroy(MyTarget);
            MyTarget = null;
        }
    }

    void playerMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"), 0.0f); //creates variable for the animation
        transform.position = Vector3.MoveTowards(transform.position,movePoint.position, moveSpeed*Time.deltaTime); //Moves the character based on the movePoint location and speed
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.1) //Checks if the character has finished moving
        {
            if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal")/2f, 0f, 0f), 0.1f, thisStopsMovement)) //Checks if there is anything that stops movement
            {
                movePoint.position +=  new Vector3(Input.GetAxisRaw("Horizontal")/2f, 0f, 0f); //moves the movePoint horizontally
                characterAnimation.SetFloat("Horizontal", movement.x); //Runs the horizontal Animation
                characterAnimation.SetFloat("Magnitude", movement.magnitude);
            }
            if(!Physics2D.OverlapCircle(movePoint.position + new Vector3( 0f, Input.GetAxisRaw("Vertical")/2f, 0f), 0.1f, thisStopsMovement))
            {
                movePoint.position += new Vector3(0f,Input.GetAxisRaw("Vertical")/2f, 0f); //moves the movePoint Vertically
                characterAnimation.SetFloat("Vertical", movement.y); //Runs the vertical Animation-
                characterAnimation.SetFloat("Magnitude", movement.magnitude);
            }
            characterAnimation.SetFloat("Vertical", movement.y); //sets the animation to finish if the player is no longer pressing the key
            characterAnimation.SetFloat("Horizontal", movement.x); 
            characterAnimation.SetFloat("Magnitude", movement.magnitude); 
            gameManager.EnemyNearby(); 
        }
    }

    public void calculateMaxHP()
    {
        if (isDirtyHP)
        {
            maxHP = Constitution.Value*10 + 10;
            healthBar.MaxValue = maxHP;
            isDirtyHP = false;
            if(lastMaxHP < maxHP)
            {
                currentHP = currentHP + (maxHP - lastMaxHP);
                healthBar.Value = currentHP;
            }
            if(currentHP > maxHP)
            {
                currentHP = maxHP;
                healthBar.Value = currentHP;
            }
            if(maxHP <= 0)
            {
                PlayerDied();
            }
            lastMaxHP = maxHP;
        }
    }
    public void PlayerDied()
    {
        gameManager.playerHasDied();
    }

    public void calculateExperience(int xp)
    {
        currentEXP = currentEXP + xp;
        experienceBar.Value = currentEXP;
        if(currentEXP >= toLevelUp[CurrentLevel])
        {
            LevelUp();
        }
    }
    private void LevelSetup()
    {
        for (int i = 1; i < toLevelUp.Length; i++)
        {
            toLevelUp[i] = (int)(Mathf.Floor(100*(Mathf.Pow(i,1.3f))));
        }
    }
    
    public void LevelUp()
    {
        LevelDirty = true;
        StatPoints = StatPoints+3;
        CurrentLevel++;
        experienceBar.MaxValue = toLevelUp[CurrentLevel];
        currentEXP = 0;
        experienceBar.Value = currentEXP;
        healingDone(maxHP);
        
    }
}