using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterClass : MonoBehaviour
{
    public float moveSpeed;

    public LayerMask thisStopsMovement;
    public Animator characterAnimation;

    public GameObject MyTarget { get; set; }
    
    [SerializeField]
    protected GameObject damagePrefab;
    protected bool isAttacking;
    protected Text damageText;
    protected int damageDealt;
    public CharacterStat Constitution, Strength, Dexterity;
    [SerializeField]
    public BarScript healthBar;
    public int maxHP;
    public int currentHP;

    public int weaponDamageMin;
    public int weaponDamageMax;
    private int IAS;
    public float attackSpeed;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        maxHP = Constitution.Value*10;
        currentHP = maxHP;
        healthBar.MaxValue = maxHP;
        healthBar.Value = currentHP;
    }


    // Update is called once per frame
    void Update()
    {

    }
    public float calculateAttackSpeed()
    {
        IAS = 100+(Dexterity.Value*4);
        attackSpeed = 250f/IAS; 
        return attackSpeed;
    }

    protected IEnumerator WeaponAttack()
    {
        if (!isAttacking)
        { 
            if (currentHP > 0)
            {
                if(this.gameObject.GetComponent<BoxCollider2D>().IsTouching(MyTarget.GetComponent<BoxCollider2D>()) && MyTarget.GetComponent<CharacterClass>().currentHP > 0)
                {
                    isAttacking = true;
                    GameObject damage = Instantiate(damagePrefab,MyTarget.transform);
                    damageDealt = damageCalculator();
                    damageText = damage.GetComponentInChildren<Text>();
                    damageText.text = damageDealt.ToString();
                    MyTarget.GetComponent<CharacterClass>().damageTaken(damageDealt);
                    if(this.tag != "Enemy" && MyTarget.GetComponent<CharacterClass>().currentHP <= 0)
                    {
                        this.GetComponent<PlayerController>().calculateExperience(MyTarget.GetComponent<Enemy>().calculateEXPValue());
                        if (MyTarget.GetComponent<Enemy>().LootBag != null)
                            MyTarget = MyTarget.GetComponent<Enemy>().LootBag;
                        else
                            MyTarget = null;
                        healingDone(3+(int)Mathf.Round(Constitution.Value*1.2f));
                    }
                    yield return new WaitForSeconds(calculateAttackSpeed()); 
                    isAttacking = false;
                    Destroy(damage); 
                }
            }
            
        }
    }

    protected int damageCalculator()
    {
        int weaponDamage = (int) Mathf.Round((Random.value*(weaponDamageMax-weaponDamageMin)) + weaponDamageMin);
        int strengthDamage = (int) Mathf.Round((Random.value*(2*Strength.Value-Strength.Value)) + Strength.Value);
        Debug.Log(weaponDamage);
        Debug.Log(strengthDamage);
        int totalDamage = weaponDamage + strengthDamage;
        return totalDamage;
    }

    protected void damageTaken(int damageTaken)
    {
        currentHP = currentHP - damageTaken;
        healthBar.Value = currentHP;
        if(currentHP <= 0)
        {
            if(this.tag == "Enemy"){
                this.GetComponent<Enemy>().EnemyDied();
            }
            else
            {
                this.GetComponent<PlayerController>().PlayerDied();
            }
        }
    }

    protected void healingDone(int healingDone)
    {
        currentHP = currentHP + healingDone;

        if(currentHP  >  maxHP)
        {
            currentHP = maxHP;
        }
        healthBar.Value = currentHP;
    }
}
