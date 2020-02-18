using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialState{
        Story01,
        Buyer,
        Story02,
        Enemy,
        Story03
        
    }
    public List<string> story_1;
    public TutorialState state;
    public float messageBoxTime = 1.0f;

    private void Start() {
        InitMessage();

        // If is tutorial then start main gameplay
        if(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            StartCoroutine(StartTutorial());
        }
    }


    public IEnumerator StartTutorial(){
        // Start the first story telling now
        state = TutorialState.Story01;
        yield return new WaitForSeconds(2.0f);
        foreach(string message in story_1) {
            yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox(message, messageBoxTime));
            // Add a buffer time
            yield return new WaitForSeconds(1.5f);
        }

        state = TutorialState.Buyer;
        // Generate some fruit and buyer now
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetFruitManager().GenerateFruit(3);
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().SpawnBuyer();

        // once buyer is clean
        while(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().getBuyerCount() != 0){
            yield return null;
        }

        state = TutorialState.Story02;
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Great! You gave wha excatly he needs!", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("However, if you give them a wrong goods \nor you ignore them for too long, \nthey might be angry at you :(.", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Now you understand better about your \ncustomer. You are excellent!", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("[Task #2]\nAnother cusotmer have came! Serve him!", messageBoxTime * 1.5f));
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().SpawnBuyerByIndex(2);

        // Wait for player kena hit
        while(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().getBuyerCount() != 0){
            yield return null;
        }
        
        state = TutorialState.Enemy;
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Seems like he don't want an apple and \nhe is angry and left your stall.", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("It is fine. You can do it better. \nYou have all my trust.", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("When you serving your buyer, some \nmercenary will appear and attack you.", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("You need to use mind-control to defend \nyour stall!", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Your stall health value is showed on the \ntable. Bear in mind...", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("A mercenary had appear! Watch out!", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("[Task #3]\nUse you mind-control to stop their attack and \nthrow it back!", messageBoxTime * 1.5f));
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().SpawnEnemy();

        // Wait for player kena hit
        while(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().getEnemyCount() != 0){
            yield return null;
        }
        
        state = TutorialState.Story03;
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Great! You have familiar yourself!", messageBoxTime));
        yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox("Get ready, more customer and mercenary is \ncoming!", messageBoxTime * 1.5f));

        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetFruitManager().GenerateFruit(6);
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().SpawnEnemy();
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().SpawnBuyer();
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().SpawnEnemy();

        while(true){
            // win tutorial
            if(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().getEnemyCount() == 0 && GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().getBuyerCount() == 0){
                GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().SetWaveEnd();
                break;
            }
            yield return true;
        }
        Debug.Log("Tutorial done");
    }

    public void InitMessage(){
        story_1.Add("You are a shopkeeper. You want to sell \nyour goods to those buyer.");
        story_1.Add("However, other merchant are just not happy \nwith you.");
        story_1.Add("They trying to stop you by hiring mercenary \nto annoy you.");
        story_1.Add("But luckily, you are a Degree Holder of \nBachelor of Magic from University of Magi.");
        story_1.Add("Interesting?");
        story_1.Add("You remembered that you know how to \nmind-control a object that is liftable.");
        story_1.Add("[Control]\nTo use mind-control, focus at the object and \nclick the button.");
        story_1.Add("[Control]\nThen, aim the target and click \nagain to shoot it out.");
        story_1.Add("[Task #1]\nYour first customer is here!\nHe is looking for a White-Apple.");
        story_1.Add("[Task #1]\nLook for a White-Apple around you and \nshoot it to the buyer right now.");
    }
}

