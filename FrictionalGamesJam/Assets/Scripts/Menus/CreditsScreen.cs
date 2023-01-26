using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    public TMPro.TMP_Text fullText;
    public TMPro.TMP_Text textShown;
    public float delayChunck;
    public float delayLetter;

    private Coroutine coroutine;
    
    void Start()
    {
        Time.timeScale = 1f;
        textShown.text = "";
        coroutine = StartCoroutine(PrintText());
    }

    IEnumerator PrintText()
    {
        string realPath = Application.dataPath;
        string[] allPathPieces = realPath.Split("/");
        string path = "";

        for(int i = 0; i < 3; i++)
        {
            path += i != 2 ? allPathPieces[i] + "\\" : allPathPieces[i];
        }

        string[] allPieces = fullText.text.Split('%');

        foreach(string piece in allPieces)
        {

            textShown.verticalAlignment = textShown.isTextOverflowing ? TMPro.VerticalAlignmentOptions.Bottom : TMPro.VerticalAlignmentOptions.Top;

            yield return new WaitForSeconds(delayChunck);

            if (piece.StartsWith("$"))
            {
                string s = piece.Remove(0, 1);

                if (s == "#")
                {
                    textShown.text += path + ">";
                }
                else
                {
                    textShown.text += s;
                }
            }
            else
            {
                foreach(char c in piece)
                {
                    yield return new WaitForSeconds(delayLetter);
                    textShown.text += c;
                }
            }
        }

        Application.Quit();
    }
}
