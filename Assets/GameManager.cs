using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //Singleton;

    [SerializeField] private GameObject character; //Reference to our character
    [SerializeField] private GameObject respawningC; //Reference to where the character respawns after dying
    [SerializeField] private GameObject enemies; //Reference of our enemy prefab
    [SerializeField] private GameObject respawnELeft; //Reference to where the enemies will be spawning from
    [SerializeField] private GameObject respawnERight; //Reference to where the enemies will be spawning from
    //private float oldTime = 0f; //Old time (0 by default) which allows us to toggle (switch) between a pause and the time scale of the game
    public bool canMoveE = true; //if enemies move or not
    [SerializeField] private Camera cameraPlayer; //camera with character visible
    [SerializeField] private Camera cameraShoot; //first person camera for shooting
    [SerializeField] private Camera cameraMenu; //camera for the menu
    [SerializeField] private Image cursorshoot; //shooting image
    //private bool sCamPlayer = false;
    private bool sCamShoot = false;
    [SerializeField] private GameObject rigidchar; //reference to the rigidbody replacing character
    private string sceneToReload = "LilasSceneS"; // Load this screen when retrying game
    private bool gameIsOn = false;

    //Menu UI
    [SerializeField] private Text title; //Reference for the title
    [SerializeField] private GameObject menuBG; //plane used for menu background
    [SerializeField] private GameObject btnReplay; //Reference for button play
    [SerializeField] private GameObject btnQuit; //Reference for button play
    [SerializeField] private GameObject btnBegin; //Reference for button Begin
    [SerializeField] private Text txtCredits; //text for credits
    [SerializeField] private Text txtCredits1; //text for credits
    [SerializeField] private Text txtCredits2; //text for credits
    [SerializeField] private Text txtCredits3; //text for credits
    [SerializeField] private Text pageCredits1; //page number for credits
    [SerializeField] private Text pageCredits2; //page number for credits
    [SerializeField] private Text pageCredits3; //page number for credits
    [SerializeField] private GameObject btnCredits1C; //Reference for button credits1 continue
    [SerializeField] private GameObject btnCredits2C; //Reference for button credits2 continue
    [SerializeField] private GameObject btnCredits3C; //Reference for button credits3 continue
    [SerializeField] private GameObject btnCredits1B; //Reference for button credits1 back
    [SerializeField] private GameObject btnCredits2B; //Reference for button credits2 back
    [SerializeField] private GameObject btnCredits3B; //Reference for button credits3 back
    [SerializeField] private Text txtRules; //text for rules
    [SerializeField] private Text txtRulesC; //text for rules
    [SerializeField] private Text pageRules; //page number for rules
    [SerializeField] private GameObject btnRulesC; //Reference for button rules continue
    [SerializeField] private GameObject btnRulesB; //Reference for button rules back
    [SerializeField] private Text txtAdditionalInfo; //text for AdditionalInfo
    [SerializeField] private Text txtAdditionalInfoC; //text for AdditionalInfo
    [SerializeField] private Text pageAdditionalInfo; //page number for AdditionalInfo
    [SerializeField] private GameObject btnAdditionalInfoC; //Reference for button AdditionalInfo continue
    [SerializeField] private GameObject btnAdditionalInfoB; //Reference for button AdditionalInfo back
    [SerializeField] private Text txtControls; //text for controls
    [SerializeField] private Text txtControlsC; //text for controls
    [SerializeField] private Text pageControls; //page number for controls
    [SerializeField] private GameObject btnControlsB; //Reference for button controls back
    [SerializeField] private GameObject btnplay; //Reference for button play

    [SerializeField] private int lives = 5; //number of lives before game over
    private const string prelives = "Lives = "; //pretext before showing the number of lives
    [SerializeField] private Text txtlives; //text for lives
    [SerializeField] private Text diedOnce; //Reference for the message when player died once and gets respawned
    [SerializeField] private Text gameOver; //Reference for the game over txt when dead
    [SerializeField] private float stamina = 100; //number of stamina for shooting
    private const string prestamina = "Stamina = "; //pretext before showing the number of stamina
    [SerializeField] private Text txtstamina; //text for stamina
    [SerializeField] private Text txtnostamina; //text for no more stamina and cannot enter shooting mode
    [SerializeField] private Text txtnoshooting; //text for no shooting when recovering stamina
    private static float recovery = 5f; //recover stamina 
    private bool healed; //after healing
    private bool instantiateMode = false;
    [SerializeField] private float instantiateTimer = 10f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    //public void SwitchCamPlayer(InputAction.CallbackContext context)
    //{
    //    sCamPlayer = context.performed;
    //}

    public void SwitchCamShoot(InputAction.CallbackContext context)
    {
        sCamShoot = context.performed;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameIsOn = false;
        cameraPlayer.enabled = false;
        cameraShoot.enabled = false;
        cursorshoot.enabled = false;
        rigidchar.SetActive(false);
        character.SetActive(false);

        cameraMenu.enabled = true;
        btnBegin.SetActive(true); //the button Begin is available
        btnQuit.SetActive(true); //the button Quit is active
        btnReplay.SetActive(false); //inactive until dead or game completed
        menuBG.SetActive(true); //menu plane is active

        txtCredits.enabled = false; //active at appropriate page of the menu
        txtCredits1.enabled = false; //active at appropriate page of the menu   
        txtCredits2.enabled = false; //active at appropriate page of the menu   
        txtCredits3.enabled = false; //active at appropriate page of the menu   
        pageCredits1.enabled = false; //active at appropriate page of the menu
        pageCredits2.enabled = false; //active at appropriate page of the menu
        pageCredits3.enabled = false; //active at appropriate page of the menu
        btnCredits1C.SetActive(false); //active at appropriate page of the menu
        btnCredits2C.SetActive(false); //active at appropriate page of the menu
        btnCredits3C.SetActive(false); //active at appropriate page of the menu
        btnCredits1B.SetActive(false); //active at appropriate page of the menu
        btnCredits2B.SetActive(false); //active at appropriate page of the menu
        btnCredits3B.SetActive(false); //active at appropriate page of the menu
        txtRules.enabled = false; //active at appropriate page of the menu
        txtRulesC.enabled = false; //active at appropriate page of the menu
        pageRules.enabled = false; //active at appropriate page of the menu
        btnRulesC.SetActive(false); //active at appropriate page of the menu
        btnRulesB.SetActive(false); //active at appropriate page of the menu
        txtAdditionalInfo.enabled = false; //active at appropriate page of the menu
        txtAdditionalInfoC.enabled = false; //active at appropriate page of the menu
        pageAdditionalInfo.enabled = false; //active at appropriate page of the menu
        btnAdditionalInfoC.SetActive(false); //active at appropriate page of the menu
        btnAdditionalInfoB.SetActive(false); //active at appropriate page of the menu
        txtControls.enabled = false; //active at appropriate page of the menu
        txtControlsC.enabled = false; //active at appropriate page of the menu
        pageControls.enabled = false; //active at appropriate page of the menu
        btnControlsB.SetActive(false); //active at appropriate page of the menu
        btnplay.SetActive(false); //not visible until last page

        txtlives.enabled = false; //text hidden
        diedOnce.enabled = false; //when player died once
        gameOver.enabled = false; //inactive unless dead and game over
        txtstamina.enabled = false; //text hidden
        txtnostamina.enabled = false; //text hidden
        txtnoshooting.enabled = false; //text hidden
    }

    public void TitlePage()
    {
        //Deactivate Pages Before and After
        txtCredits.enabled = false; //active at appropriate page of the menu
        txtCredits1.enabled = false; //active at appropriate page of the menu
        pageCredits1.enabled = false; //active at appropriate page of the menu
        btnCredits1C.SetActive(false); //active at appropriate page of the menu
        btnCredits1B.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        title.enabled = true; //title is visible
        btnQuit.SetActive(true); //quitting is an option
        btnBegin.SetActive(true); //the button Begin is available
    }

    public void CreditsPage1()
    {
        //Deactivate Pages Before and After
        title.enabled = false; //title is invisible
        btnQuit.SetActive(false); //inactive
        btnBegin.SetActive(false); //ianctive
        txtCredits2.enabled = false; //active at appropriate page of the menu
        pageCredits2.enabled = false; //active at appropriate page of the menu
        btnCredits2C.SetActive(false); //active at appropriate page of the menu
        btnCredits2B.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        txtCredits.enabled = true; //active at appropriate page of the menu
        txtCredits1.enabled = true; //active at appropriate page of the menu   
        pageCredits1.enabled = true; //active at appropriate page of the menu
        btnCredits1C.SetActive(true); //active at appropriate page of the menu
        btnCredits1B.SetActive(true); //active at appropriate page of the menu
    }

    public void CreditsPage2()
    {
        //Deactivate Pages Before and After
        txtCredits1.enabled = false; //active at appropriate page of the menu
        pageCredits1.enabled = false; //active at appropriate page of the menu
        btnCredits1C.SetActive(false); //active at appropriate page of the menu
        btnCredits1B.SetActive(false); //active at appropriate page of the menu
        txtCredits3.enabled = false; //active at appropriate page of the menu 
        pageCredits3.enabled = false; //active at appropriate page of the menu
        btnCredits3C.SetActive(false); //active at appropriate page of the menu
        btnCredits3B.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        txtCredits.enabled = true; //active at appropriate page of the menu
        txtCredits2.enabled = true; //active at appropriate page of the menu   
        pageCredits2.enabled = true; //active at appropriate page of the menu
        btnCredits2C.SetActive(true); //active at appropriate page of the menu
        btnCredits2B.SetActive(true); //active at appropriate page of the menu
    }

    public void CreditsPage3()
    {
        //Deactivate Pages Before and After
        txtCredits2.enabled = false; //active at appropriate page of the menu
        pageCredits2.enabled = false; //active at appropriate page of the menu
        btnCredits2C.SetActive(false); //active at appropriate page of the menu
        btnCredits2B.SetActive(false); //active at appropriate page of the menu
        txtRules.enabled = false; //active at appropriate page of the menu
        txtRulesC.enabled = false; //active at appropriate page of the menu
        pageRules.enabled = false; //active at appropriate page of the menu
        btnRulesC.SetActive(false); //active at appropriate page of the menu
        btnRulesB.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        txtCredits.enabled = true; //active at appropriate page of the menu
        txtCredits3.enabled = true; //active at appropriate page of the menu   
        pageCredits3.enabled = true; //active at appropriate page of the menu
        btnCredits3C.SetActive(true); //active at appropriate page of the menu
        btnCredits3B.SetActive(true); //active at appropriate page of the menu
    }

    public void RulesPage()
    {
        //Deactivate Pages Before and After
        txtCredits.enabled = false; //active at appropriate page of the menu
        txtCredits.enabled = false; //active at appropriate page of the menu
        txtCredits3.enabled = false; //active at appropriate page of the menu   
        pageCredits3.enabled = false; //active at appropriate page of the menu
        btnCredits3C.SetActive(false); //active at appropriate page of the menu
        btnCredits3B.SetActive(false); //active at appropriate page of the menu
        txtAdditionalInfo.enabled = false; //active at appropriate page of the menu
        txtAdditionalInfoC.enabled = false; //active at appropriate page of the menu
        pageAdditionalInfo.enabled = false; //active at appropriate page of the menu
        btnAdditionalInfoC.SetActive(false); //active at appropriate page of the menu
        btnAdditionalInfoB.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        txtRules.enabled = true; //active at appropriate page of the menu
        txtRulesC.enabled = true; //active at appropriate page of the menu
        pageRules.enabled = true; //active at appropriate page of the menu
        btnRulesC.SetActive(true); //active at appropriate page of the menu
        btnRulesB.SetActive(true); //active at appropriate page of the menu
    }

    public void AdditionalInfoPage()
    {
        //Deactivate Pages Before and After
        txtRules.enabled = false; //active at appropriate page of the menu
        txtRulesC.enabled = false; //active at appropriate page of the menu
        pageRules.enabled = false; //active at appropriate page of the menu
        btnRulesC.SetActive(false); //active at appropriate page of the menu
        btnRulesB.SetActive(false); //active at appropriate page of the menu
        txtControls.enabled = false; //active at appropriate page of the menu
        txtControlsC.enabled = false; //active at appropriate page of the menu
        pageControls.enabled = false; //active at appropriate page of the menu
        btnControlsB.SetActive(false); //active at appropriate page of the menu
        btnplay.SetActive(false); //not visible until last page

        //Active in this page
        txtAdditionalInfo.enabled = true; //active at appropriate page of the menu
        txtAdditionalInfoC.enabled = true; //active at appropriate page of the menu
        pageAdditionalInfo.enabled = true; //active at appropriate page of the menu
        btnAdditionalInfoC.SetActive(true); //active at appropriate page of the menu
        btnAdditionalInfoB.SetActive(true); //active at appropriate page of the menu
    }

    public void ControlsPage()
    {
        //Deactivate Page Before
        txtAdditionalInfo.enabled = false; //active at appropriate page of the menu
        txtAdditionalInfoC.enabled = false; //active at appropriate page of the menu
        pageAdditionalInfo.enabled = false; //active at appropriate page of the menu
        btnAdditionalInfoC.SetActive(false); //active at appropriate page of the menu
        btnAdditionalInfoB.SetActive(false); //active at appropriate page of the menu

        //Active in this page
        txtControls.enabled = true; //active at appropriate page of the menu
        txtControlsC.enabled = true; //active at appropriate page of the menu
        pageControls.enabled = true; //active at appropriate page of the menu
        btnControlsB.SetActive(true); //active at appropriate page of the menu
        btnplay.SetActive(true); //not visible until last page
    }

    public void GameStart()
    {
        gameIsOn = true;
        InvokeRepeating("SpawnEnemies", 0, 10);
        Cursor.lockState = CursorLockMode.Locked;
        cameraPlayer.enabled = true;
        character.SetActive(true);

        cameraMenu.enabled = false;
        menuBG.SetActive(false);
        txtControls.enabled = false; //active at appropriate page of the menu
        txtControlsC.enabled = false; //active at appropriate page of the menu
        pageControls.enabled = false; //active at appropriate page of the menu
        btnControlsB.SetActive(false); //active at appropriate page of the menu
        btnplay.SetActive(false); //not visible until last page

        txtlives.enabled = true; //activate text
        txtlives.text = prelives + lives.ToString("D1"); //show lives
        txtstamina.enabled = true; //activate text
        txtstamina.text = prestamina + Mathf.FloorToInt(stamina).ToString("D3"); //show stamina
    }

    public void Quit()
    {
        Application.Quit(); //Terminate application
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneToReload); //Load the game scene
    }

    public void Dead()
    {
        lives--;
        diedOnce.enabled = true; //activate message
        Invoke("DiedOff", 3); //message disappears
        txtlives.text = prelives + lives.ToString("D1"); //show lives
        character.transform.position = respawningC.transform.position;
        rigidchar.transform.position = respawningC.transform.position;
        healed = false;
        CancelInvoke("SpawnEnemies");
        canMoveE = false;
        instantiateMode = false;
        instantiateTimer = 10f;
        InvokeRepeating("SpawnEnemies", 30, 10);
        if(lives == 0)
        {
            gameIsOn = false;
            diedOnce.enabled = false; //if game over, do not show message
            Cursor.lockState = CursorLockMode.None;
            gameOver.enabled = true; //active
            btnReplay.SetActive(true); //inactive until dead or game completed
            btnQuit.SetActive(true); //the button Quit is active
            character.SetActive(false); //character disappears
        }
    }

    public void DiedOff()
    {
        diedOnce.enabled = false; //deactivate
    }

    public void SpawnEnemies()
    {
        canMoveE = true;
        GameObject e = Instantiate(enemies, respawnELeft.transform.position, respawnELeft.transform.rotation);
        GameObject e2 = Instantiate(enemies, respawnERight.transform.position, respawnERight.transform.rotation);
    }
    
    public void SpawnEnemiesWithoutInvoke()
    {
        //Reference 2
        instantiateTimer -= Time.deltaTime;
        if (Mathf.FloorToInt(instantiateTimer) == 0)
        {
            canMoveE = true;
            GameObject e = Instantiate(enemies, respawnELeft.transform.position, respawnELeft.transform.rotation);
            GameObject e2 = Instantiate(enemies, respawnERight.transform.position, respawnERight.transform.rotation);
            instantiateTimer = 10f;
        }
    }

    public void LoseStamina()
    {
        if (Mathf.FloorToInt(stamina) > 0)
        {
            stamina -= 5;
        }
        
        txtstamina.text = prestamina + Mathf.FloorToInt(stamina).ToString(); //show stamina
        if (Mathf.FloorToInt(stamina) >= 0 && Mathf.FloorToInt(stamina) < 5) 
        {
            txtnostamina.enabled = true;
            Invoke("NoMoreStamina", 3);
            sCamShoot = false;
            character.transform.position = rigidchar.transform.position;
            character.SetActive(true);
            rigidchar.SetActive(false);
            cameraPlayer.enabled = true;
            cameraShoot.enabled = false;
            cursorshoot.enabled = false;
        }
    }

    public void NoMoreStamina()
    {
        txtnostamina.enabled = false; //deactivate
    }

    public void ReminderNoStamina()
    {
        txtnostamina.enabled = true;
        Invoke("NoMoreStamina", 2);
    }
    
    public void NoShooting()
    {
        txtnoshooting.enabled = false; //deactivate
    }

    public void ReminderNoShooting()
    {
        txtnoshooting.enabled = true;
        Invoke("NoShooting", 2);
    }

    public bool NoShoot()
    {
        if(Mathf.FloorToInt(stamina) >= 0 && Mathf.FloorToInt(stamina) < 5)
        {
            return true;
        }
        return false;
    }

    //OnClick for message when collecting an object
    public void ConfirmMessage()
    {
        //Message disappears
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOn)
        {
            //if (sCamPlayer)
            //{
            //    character.transform.position = rigidchar.transform.position;
            //    sCamShoot = false;
            //    character.SetActive(true);
            //    rigidchar.SetActive(false);
            //    cameraPlayer.enabled = true;
            //    cameraShoot.enabled = false;
            //    cursorshoot.enabled = false;
            //}

            if (Mathf.FloorToInt(stamina) >= 5)
            {
                if (sCamShoot)
                {
                    rigidchar.transform.position = character.transform.position;
                    //sCamPlayer = false;
                    character.SetActive(false);
                    rigidchar.SetActive(true);
                    cameraShoot.enabled = true;
                    cameraPlayer.enabled = false;
                    cursorshoot.enabled = true;
                }
                else
                {
                    //character.transform.position = rigidchar.transform.position;
                    sCamShoot = false;
                    character.SetActive(true);
                    rigidchar.SetActive(false);
                    cameraPlayer.enabled = true;
                    cameraShoot.enabled = false;
                    cursorshoot.enabled = false;
                }
            }
            else if (Mathf.FloorToInt(stamina) >= 0 && Mathf.FloorToInt(stamina) < 5)
            {
                if (sCamShoot)
                {
                    ReminderNoStamina();
                    sCamShoot = false;
                    rigidchar.SetActive(false);
                    cameraPlayer.enabled = true;
                    cameraShoot.enabled = false;
                    cursorshoot.enabled = false;
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(character.transform.position, Vector3.down, out hit, 5))
            {
                if (hit.collider.tag == "Heal") //Heal object
                {
                    CancelInvoke("SpawnEnemies");
                    instantiateMode = false;
                    instantiateTimer = 10f;
                    canMoveE = false;
                    healed = true;
                    if (Mathf.FloorToInt(stamina) < 100)
                    {
                        stamina += recovery * Time.deltaTime;
                    }

                    txtstamina.text = prestamina + Mathf.FloorToInt(stamina).ToString(); //show stamina

                    if (sCamShoot)
                    {
                        ReminderNoShooting();
                        sCamShoot = false;
                        character.transform.position = rigidchar.transform.position;
                        character.SetActive(true);
                        rigidchar.SetActive(false);
                        cameraPlayer.enabled = true;
                        cameraShoot.enabled = false;
                        cursorshoot.enabled = false;
                    }
                }
                else if (!canMoveE && healed && !instantiateMode)
                {
                    healed = false;
                    instantiateMode = true;
                    canMoveE = true;
                }
            }

            if (instantiateMode)
            {
                SpawnEnemiesWithoutInvoke();
                CancelInvoke("SpawnEnemies");
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            sCamShoot = false;
            cameraShoot.enabled = false;
            cursorshoot.enabled = false;
        }

        //if (Input.GetButtonDown("Cancel")) //Pausing the game with button Escape
        //{
        //    //Reference 1
        //    float prevTime = oldTime; //Hide the saved timeScaled
        //    oldTime = Time.timeScale; //Permute (alter) the timeScale so we can come back to it
        //    Time.timeScale = prevTime; //Change timeScale for the hidden value
        //}
    }
}

//References
//1- Script for pausing the game in Game Engine I class project Rebonds : https://youtu.be/4fmy_ymj6jE?t=3332
//2- https://answers.unity.com/questions/637597/instantiate-at-intervals.html (for instantiating prefabs at a certain interval to replace InvokeRepeating)