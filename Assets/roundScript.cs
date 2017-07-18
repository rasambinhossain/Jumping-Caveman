using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class roundScript : MonoBehaviour {

   
    [Header("Round Settings")]
    [Tooltip("Round Number")]
    public string roundNumber;
    Button playGame;
    Image panelImage;

    Text roundText;
    void Start()
    {
        playGame = GameObject.Find("playGame").GetComponentInChildren<Button>();
        playGame.image.enabled = false;
        playGame.enabled = false;
        StartCoroutine(animateText(roundNumber));
    }

    private IEnumerator animateText(string strComplete)
    {
        int i = 0;
        string str = "";
        while (i < strComplete.Length)
        {
            str += strComplete[i++];
            yield return new WaitForSeconds(0.5F);
            roundText = GetComponent<Text>();
            roundText.text = str;
        }
        StartCoroutine(gamePause(0.5F));    // short pause before starting the next scene      
    }

    private IEnumerator gamePause(float v)
    {
        yield return new WaitForSeconds(1);
        roundText.enabled = false;
        playGame.image.enabled = true;
        playGame.enabled = true;
        panelImage = GameObject.Find("panel").GetComponentInChildren<CanvasRenderer>().GetComponent<Image>();
        if (panelImage.color.a != 1)
        {
            StartCoroutine(playGameAnimation(0.025F));
        }

        //playGame.image.enabled = true;
        //playGame.enabled = true;
        //SceneManager.LoadScene("MainScene");
    }
    private IEnumerator playGameAnimation(float v)
    {
        yield return new WaitForSeconds(v);
        panelImage.color = new Color(0, 0, 0, panelImage.color.a + v);
        //panelImage = GameObject.Find("panel").GetComponentInChildren<CanvasRenderer>().GetComponent<Image>();
        if (panelImage.color.a != 1)
            StartCoroutine(playGameAnimation(0.025F));
    }
}
