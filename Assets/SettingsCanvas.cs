using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsCanvas : MonoBehaviour
{
    public GameObject dialoguePanel;
    private GameObject escPanel;

    public GameObject player;
    public GameObject mapGrid;
    // Start is called before the first frame update
    void Start()
    {
        escPanel = transform.GetChild(0).gameObject;
    }

    private void OnEnable() {
        Time.timeScale = 0;
        //PlayerManager.isControlling=false;
    }
    private void OnDisable() {
        Time.timeScale=1;
        //PlayerManager.isControlling=true;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            escPanel.SetActive(!escPanel.activeSelf);
            PlayerManager.isControlling=!escPanel.activeSelf;
            if(escPanel.activeSelf){
                Time.timeScale=0;
            }
            else{
                Time.timeScale=1;
            }
        }
    }
    public void ReviewDialogue(){
        escPanel.SetActive(false);
        dialoguePanel.SetActive(true);

    }
    public void ExitGame(){
        //save game

        Debug.Log("quit");
        Application.Quit();
    }
    public void RestartGame(){
        SceneManager.LoadScene(0);
        //player.SetActive(true);
        //player.GetComponent<CharacterActions>().CharacterReset();
        //mapGrid.GetComponent<MapControl>().nextlevel();
        
    }
}
