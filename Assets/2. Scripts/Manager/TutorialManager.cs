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
        Story03,
        EnemyAttack,
        
    }
    public List<string> story_1;
    

    private void Start() {
        InitMessage();

        // If is tutorial then start main gameplay
        if(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetGameStateManager().getIsTutorial()){
            StartCoroutine(StartTutorial());
        }
    }

    public IEnumerator StartTutorial(){
        // Start the first story telling now
        yield return new WaitForSeconds(2.0f);
        foreach(string message in story_1) {
            yield return StartCoroutine(GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetMessageBoxManager().SummonBox(message, 2.0f));
            // Add a buffer time
            yield return new WaitForSeconds(1.5f);
        }

        // Generate some fruit and buyer now
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetFruitManager().GenerateFruit(3);
        // GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetEnemyManager().SpawnEnemy();
        GameObject.Find("GlobalManager").GetComponent<GlobalManager>().GetBuyerManager().SpawnBuyer();
    }

    public void InitMessage(){
        story_1.Add("You are a shopkeeper. You want to sell \nyour goods to those buyer.");
        story_1.Add("However, other merchant are just not happy \nwith you.");
        story_1.Add("They trying to stop you by hiring mercenary \nto annoy you.");
        story_1.Add("But luckily, you are a Degree Holder of \nBachelor of Magic from University of Magi.");
        story_1.Add("Interesting?");
        story_1.Add("You remembered that you know how to \nmind-control a object that is liftable.");
        story_1.Add("To cast a magic, focus at the object.\nClick the button to lift it up.\nClick second time to shoot it out.");
        story_1.Add("Your first customer is here!\nHe is looking for a White-Apple.\nLook for a White-Apple and Shoot it to the \nbuyer right now.");
    }
}
