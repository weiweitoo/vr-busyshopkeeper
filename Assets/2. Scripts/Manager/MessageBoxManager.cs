using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxManager : MonoBehaviour
{
    public GameObject messageBoxCanvas;
    public GameObject messageBoxPrefab;
    public GameObject scoreboardPrefab;

    public IEnumerator SummonBox(string text, float time){
        GameObject generatedObject = Instantiate(messageBoxPrefab, messageBoxCanvas.transform.position, Quaternion.identity);
        Transform messageText = generatedObject.transform.Find("MessageText");
        Text textComponent = messageText.GetComponent<Text>();
        textComponent.text = text;
        generatedObject.transform.SetParent(messageBoxCanvas.transform, false);
        yield return new WaitForSeconds(time);
        Destroy(generatedObject);
    }

    public IEnumerator SummonScoreBoard(string text, float time){
        GameObject generatedObject = Instantiate(scoreboardPrefab, messageBoxCanvas.transform.position, Quaternion.identity);
        Transform messageText = generatedObject.transform.Find("MessageText");
        Text textComponent = messageText.GetComponent<Text>();
        textComponent.text = text;
        generatedObject.transform.SetParent(messageBoxCanvas.transform, false);
        yield return new WaitForSeconds(time);
        Destroy(generatedObject);
    }
}
