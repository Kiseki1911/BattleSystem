using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FollowDialogControl : MonoBehaviour
{
    public GameObject followCanvas;
    public TMP_Text textLabel;
    public Image characterImage;
    public TextAsset textFile;
    public float textSpeed;

    public int index;
    List<string> textList = new List<string>();
    private void Awake()
    {
        //GetTextFromFile(textFile);
        
    }
    private void OnEnable() {
        textLabel.text = textList[index];
        index++;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space))&&index==textList.Count){
            followCanvas.SetActive(false);
            index=0;
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
    public void GetText(string[] lines){
        textList.Clear();
        index = 0;
        for(int i =0; i <lines.Length;i++){
            textList.Add(lines[i]);
        }
        //var lineData = file.text.Split('\n');
    }
    public void SkipButton(){
        followCanvas.SetActive(false);
    }
}
