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

    public int index=0;
    List<string> textList = new List<string>();
    string[] starting = new string[]{"(头好痛...这是哪...)",
                                    "*我叫诸仁恭,二十四岁,是学...是个专业的房地产中介.*",
                                    "*根据过去五分钟我对附近周围的观察,我好像穿越了.*",
                                    "(既然穿越了,那请给我无敌的外挂和萌妹女仆!!!)",
                                    "(哼 哼 哼 啊---)*你大声呼叫*",
                                    "*可是没有人回应*",
                                    "(可恶...行不通啊...等等,这是?)",
                                    "*你大声咆哮时误触了[I]键打开了背包*",
                                    "(奇怪的打造系统?这就是属于我的外挂吗.不管怎么样,属于老子的专属冒险就此开始吧!)"};
    private void Awake()
    {
        //GetTextFromFile(textFile);
        Review();
    }
    private void OnEnable() {
        //textLabel.text = textList[index];
        //index++;
    }

    // Update is called once per frame
    void Update()
    {
        if(followCanvas.activeSelf){
            if((Input.GetKeyDown(KeyCode.Space))&&index==textList.Count){
                followCanvas.SetActive(false);
                PlayerManager.isControlling=true;
                Time.timeScale=1;
                index=0;
                return;
            }
            if((Input.GetKeyDown(KeyCode.Space))){
                textLabel.text = textList[index];
                index++;
            }
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
        textLabel.text = textList[index];
        index++;
        followCanvas.SetActive(true);
        //var lineData = file.text.Split('\n');
    }
    public void Review(){
        GetText(starting);
    }

    public void SkipButton(){
        followCanvas.SetActive(false);
    }
}
