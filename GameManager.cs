using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject deathCanvas, target;
    private GameObject[] enemies;
    public bool GamePaused;

    
    // Start is called before the first frame update
    void Start()
    {
        deathCanvas.SetActive(false);
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
        GamePaused = false;
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        GameIsPaused();
    }

    private void GameIsPaused()
    {
        if(GamePaused)
        {
            player.enabled = false;
        }
        else
        {
            player.enabled = true;
        }
    }

    public void playerHasDied()
    {
        deathCanvas.SetActive(true);
        player.enabled = false;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void MainMenu()
    {
        Application.Quit();
    }
    
    public void ClickTarget()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit =  Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero , Mathf.Infinity);
            if(hit.collider != null)
            {
                Debug.Log(hit.collider);
                if(hit.collider.tag == "Enemy" || hit.collider.tag == "Lootable" || hit.collider.tag == "Interactable")
                {
                    player.MyTarget = hit.collider.gameObject;  
                    target.transform.position = player.MyTarget.transform.position;
                    if (player.MyTarget.tag == "Enemy")
                    {
                        target.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                        target.SetActive(true);
                    }
                    else if (player.MyTarget.tag == "Lootable" || player.MyTarget.tag == "Interactable")
                    {
                        target.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                        target.SetActive(true);
                    }
                }      
            }   
            else
            {
                player.MyTarget = null;
                target.SetActive(false);
            }
        }
    }
    
    public void EnemyNearby()
    {
        Vector3 playerPosition = player.gameObject.transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - playerPosition;
            float curDistance = diff.sqrMagnitude;
            if(enemy.tag == "Enemy")  
            {
                if (curDistance <= 9)
                {
                    enemy.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    enemy.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
