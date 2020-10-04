using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string tutorial;
    public string game;
    public string won;
    public string lost;
    public string tie;

    private string currentscene;

    public GameObject[] tutorialScenes;
    private int i = 0;

    void Start()
    {
        DontDestroyOnLoad(this);
        currentscene = tutorial;
        foreach (GameObject page in tutorialScenes) {
            page.SetActive(false);
        }
        tutorialScenes[0].SetActive(true);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentscene == tutorial)
        {
            if (i < tutorialScenes.Length - 1)
            {
                tutorialScenes[i].SetActive(false);
                i += 1;
                tutorialScenes[i].SetActive(true);
            }
            else
            {
                currentscene = game;
                SceneManager.LoadScene(game);
            }
        }
        else if (Input.GetMouseButtonDown(1) && currentscene == tutorial)
        {
            currentscene = game;
            SceneManager.LoadScene(game);

        }
        else if (Input.GetMouseButtonDown(0) && (currentscene == won || currentscene == lost || currentscene == tie)) {
            currentscene = tutorial;
            SceneManager.LoadScene(tutorial);
            Destroy(this);
        }
    }

    public void WonScene() {
        currentscene = won;
        SceneManager.LoadScene(won);
    }

    public void LostScene()
    {
        currentscene = lost;
        SceneManager.LoadScene(lost);
    }

    public void TieScene()
    {
        currentscene = tie;
        SceneManager.LoadScene(tie);
    }
}

