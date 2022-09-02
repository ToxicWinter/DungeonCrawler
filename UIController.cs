using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameCanvas, characterCanvas, deathCanvas, storyCanvas, instructions;
    [SerializeField]
    private Text strValue, dexValue, conValue, statValue, strTmpText, dexTmpText, conTmpText, hpText, damageText, speedText, deathText;
    [SerializeField]
    private Button strUp, strDown, dexUp, dexDown, conUp, conDown, confirmLevelUpButton, resetLevelUp;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerController player;
    public bool PlayerDead = false;
    private int strTmp, dexTmp, conTmp, damageMinText, damageMaxText;
    // Start is called before the first frame update

    void Start()
    {
        gameCanvas.SetActive(true);
        instructions.SetActive(true);
        characterCanvas.SetActive(false);
        deathCanvas.SetActive(false);  
        storyCanvas.SetActive(false); 
        strTmp = 0;
        dexTmp = 0;
        conTmp = 0;
        UpdateStats(); 
        strValue.text = player.Strength.Value.ToString();
        dexValue.text = player.Dexterity.Value.ToString();
        conValue.text = player.Constitution.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
      playerHasDied(); 
      toggleCharacterInterface();   
      toggleLevelUp();
      hpText.text = "HP: " + player.currentHP.ToString() + "/" + player.maxHP.ToString(); //Lesson 9
      if(Input.GetKeyDown(KeyCode.Escape))
      {
        if(!gameManager.GamePaused)
        { 
            deathCanvas.SetActive(true);
            deathText.text = "";
            gameCanvas.SetActive(false);
            characterCanvas.SetActive(false);
            storyCanvas.SetActive(false); 
            player.enabled = false;
            gameManager.GamePaused = true;
        }
        else
        {
            deathCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            characterCanvas.SetActive(false);
            storyCanvas.SetActive(false); 
            player.enabled = true;
            gameManager.GamePaused = false;
        }
            
      }
    }

    private void toggleCharacterInterface()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(!gameManager.GamePaused)
            {
                gameCanvas.SetActive(false);
                characterCanvas.SetActive(true);
                storyCanvas.SetActive(false); 
                gameManager.GamePaused = true;
            }  
            else if (Input.GetKeyDown(KeyCode.C))
            {
                gameCanvas.SetActive(true);
                characterCanvas.SetActive(false); 
                storyCanvas.SetActive(false);
                gameManager.GamePaused = false;
            }   
        }
    }
    public void CloseInterface()
    {
        gameCanvas.SetActive(true);
        characterCanvas.SetActive(false);
        gameManager.GamePaused = false;
    }

    private void playerHasDied()
    {
        if(PlayerDead)
        {
            deathCanvas.SetActive(true);
            deathText.text = "You Died";
            player.enabled = false;
            gameManager.GamePaused = true;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //Lesson 9
    private void calcDamageRange()
    {
        damageMinText = player.weaponDamageMin + player.Strength.Value;
        damageMaxText = player.weaponDamageMax + (player.Strength.Value*2);
    }

    public void UpdateStats()
    {
        strTmpText.text = strTmp.ToString();
        dexTmpText.text = dexTmp.ToString();
        conTmpText.text = conTmp.ToString();
        statValue.text = player.StatPoints.ToString();
        strValue.text = player.Strength.Value.ToString();
        dexValue.text = player.Dexterity.Value.ToString();
        conValue.text = player.Constitution.Value.ToString();
        //Lesson 9
        calcDamageRange();
        player.calculateAttackSpeed();
        damageText.text = "Damage: " + damageMinText.ToString() + " - " +  damageMaxText.ToString();
        speedText.text = "Att Speed: " + player.attackSpeed.ToString("#.##")+"s";
        
    }

    public void ConfirmLevelUp(){
        player.Strength.AddModifier(new StatModifier(strTmp));
        player.Dexterity.AddModifier(new StatModifier(dexTmp));
        player.Constitution.AddModifier(new StatModifier(conTmp));
        conTmp = 0;
        dexTmp = 0;
        strTmp = 0;
        player.isDirtyHP = true;
        player.LevelDirty = false;
        UpdateStats();
        
    }

    private void toggleLevelUp()
    {
        if(player.LevelDirty)
        {
            statValue.text = player.StatPoints.ToString();
            if (player.StatPoints == 0)
            {
                strUp.interactable = false;
                dexUp.interactable = false;
                conUp.interactable = false;
                confirmLevelUpButton.interactable = true;
            }
            else 
            {
                strUp.interactable = true;
                dexUp.interactable = true;
                conUp.interactable = true;
                confirmLevelUpButton.interactable = false;
                resetLevelUp.interactable = true;
            }
            if (strTmp == 0)
            {
                strDown.interactable = false;
            }
            else
            {
                strDown.interactable = true;
            }
            if (dexTmp == 0)
            {
                dexDown.interactable = false;
            }
            else
            {
                dexDown.interactable = true;
            }
            if (conTmp == 0)
            {
                conDown.interactable = false;
            }
            else
            {
                conDown.interactable = true;
            }
        }
        else
        {
            strUp.interactable = false;
            dexUp.interactable = false;
            conUp.interactable = false;
            strDown.interactable = false;
            dexDown.interactable = false;
            conDown.interactable = false;
            confirmLevelUpButton.interactable = false;
            resetLevelUp.interactable = false;
        }
    }

    public void StrUp()
    {
        strTmp++;
        player.StatPoints--;
        UpdateStats();

    }
    public void DexUp()
    {
        dexTmp++;
        player.StatPoints--;
        UpdateStats();
    }
    public void ConUp()
    {
        conTmp++;
        player.StatPoints--;
        UpdateStats();
    }
    public void StrDown()
    {
        strTmp--;
        player.StatPoints++;
        UpdateStats();
    }
    public void DexDown()
    {
        dexTmp--;
        player.StatPoints++;
        UpdateStats();   
    }
    public void ConDown()
    {
        conTmp--;
        player.StatPoints++;
        UpdateStats();   
    }
    public void ResetLevelUp()
    {
        player.StatPoints = player.StatPoints + dexTmp + strTmp + conTmp;
        dexTmp = 0;
        strTmp = 0;
        conTmp = 0;
        UpdateStats();  
    }
    public void CloseStory()
    {
        storyCanvas.SetActive(false);
        gameManager.GamePaused = false;
    }

    public void CloseInstructions()
    {
        instructions.SetActive(false);
        gameManager.GamePaused = false;
    }
}