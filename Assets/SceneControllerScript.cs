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

    void Start()
    {
        DontDestroyOnLoad(this);
        currentscene = tutorial;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentscene == tutorial)
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

