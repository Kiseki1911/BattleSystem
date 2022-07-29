using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text textLabel;
    public Image characterImage;
    public TextAsset textFile;
    public float textSpeed;

    public int index;
    List<string> textList = new List<string>();
    private void Awake()
    {
        GetTextFromFile(textFile);
        
    }
    private void OnEnable() {
        textLabel.text = textList[index];
        index++;
        Time.timeScale=0;
        PlayerManager.isControlling=false;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space))&&index==textList.Count){
            gameObject.SetActive(false);
            index=0;
            Time.timeScale=1;
            PlayerManager.isControlling=true;
            return;
        }
        if((Input.GetKeyDown(KeyCode.Space))){
            textLabel.text = textList[index];
            index++;
        }
    }
    void GetTextFromFile(TextAsset file){
        textList.Clear();
        index = 0;
        var lineData = file.text.Split('\n');
        foreach(var line in lineData){
            textList.Add(line);
        }
    }
}
