using UnityEngine;
using TMPro;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Build.Player;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    [Header("Refrences")]
    public TMP_Text uiTextScore;
    public TMP_Text uiTextHighscore;
    public GameObject scoresParant;
    public TMP_Text scorePrefab;

    [Header("Score")]
    private int currentScore = 0;
    public int Highscore = 0;

    [Header("Highscores")]
    private List<string> names = new List<string>();
    private List<int> scores = new List<int>();
    public int maxScoresCount = 10;


    public void Awake()
    {



        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        HighscoreData data = JsonSaveLoad.Load();
        scores = data.scores.ToList();
        names = data.names.ToList();

        CleanUpHighscores();
        RefreshScoreDisplay();
    }



    private void OnDestroy()
    {
        AddHighScore(CurrentScore);
        HighscoreData data = new HighscoreData(scores.ToArray(), names.ToArray());
        JsonSaveLoad.Save(data);
    }

    public int CurrentScore
    {
        // accessing it, ex int x = CurrentScore;
        get => currentScore;

        private set // changing it
        {
            currentScore = value;
            uiTextScore.text = "Score: " + currentScore;
        }
    }
    // public int xExample { get; set; }

    public void IncreaseScore(int amount)
    {
        CurrentScore += amount;

    }

    public void RefreshScoreDisplay()
    {
        DestroyAllChildren(scoresParant);

        for (int i = 0; i < scores.Count; i++)
        {
            TMP_Text uiText = Instantiate(scorePrefab, scoresParant.transform);
            uiText.text = names[i] + " " + scores[i];

            Debug.Log(names[i] + " got " + scores[i]);
        }
    }

    private void DestroyAllChildren(GameObject parant)
    {
        Transform[] children = parant.GetComponentsInChildren<Transform>(true);

        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] == parant.transform) continue;
            Destroy(children[i].gameObject);
        }
    }

    public void AddHighScore(int score)
    {
        string[] possibleNames = new[] { "Tommy", "Timathy", "TimTam", "Tom Tom", "TommyTheBoi", "squidkid69" };
        string randomName = possibleNames[Random.Range(0, possibleNames.Length)];

        AddHighScore(randomName, score);
    }

    public void AddHighScore(string name, int score)
    {
        CleanUpHighscores();

        for (int i = 0; i < scores.Count; i++)
        {
            if (score > scores[i])
            {
                scores.Insert(i, score);
                names.Insert(i, name);

                return;
            }
        }

        if (scores.Count < maxScoresCount)
        {
            scores.Add(score);
            names.Add(name);
        }

    }

    private void CleanUpHighscores()
    {

        for(int i = maxScoresCount; i < scores.Count; i++)
        {
            names.RemoveAt(i);
            scores.RemoveAt(i);
        }
    }

}
