using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform tutorialsParent;
    List<Transform> tutorials = new List<Transform>();
    public bool inTutorial;
    public bool tutorialComplete = false;

    private float timeInTutorial = 2f;
    private float timeStartingTutorial;

    Transform tutorialActive;

    public GameObject swipeTutorial;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in tutorialsParent)    //Se añade cada tutorial a la lista de tutoriales   
        {
            child.gameObject.SetActive(false);
            tutorials.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inTutorial)
        {
            CheckTutorialComplete(tutorialActive);
            if (tutorialComplete || PlayerManager.gameOver ||  Time.time - timeStartingTutorial >= timeInTutorial)
            {
                tutorialActive.gameObject.SetActive(false);
            }
        }
        
    }

    public void TutorialActivate(string tutorialName)
    {
        inTutorial = true;
        tutorialComplete = false;
        timeStartingTutorial = Time.time;
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].name == tutorialName)
            {
                tutorialActive = tutorials[i];
                tutorialActive.gameObject.SetActive(true);
                break;
            }
        }
    }

    //Condiciones para completar los tutoriales
    void CheckTutorialComplete(Transform tutorial)
    {
        string tutorialName = tutorial.gameObject.name;
        switch (tutorialName)
        {
            case "Swipe":
                if (SwipeManager.swipeRight || SwipeManager.swipeLeft)
                {
                    tutorialActive.gameObject.SetActive(false);
                    tutorialComplete = true;
                }
                break;
            case "Jump":
                if (SwipeManager.swipeUp)
                {
                    tutorialActive.gameObject.SetActive(false);
                    tutorialComplete = true;
                }
                break;
            case "Slide":
                if (SwipeManager.swipeDown)
                {
                    tutorialActive.gameObject.SetActive(false);
                    tutorialComplete = true;
                }
                break;
            default:
                break;
        }
    }
}
