using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

    public Button startButton;
    public TextMesh textMesh;
    public Button quitButton;
    public Button tutorialButton;

    // Use this for initializatsion
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        Button btn = startButton.GetComponent<Button>();
        Button btn1 = quitButton.GetComponent<Button>();

        btn.onClick.AddListener(LoadLevel);
        btn1.onClick.AddListener(QuitGame);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("forest");
    }

    public void QuitGame()
    {
        textMesh.text = "You finish what \nyou start son!";
        // Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}
    
}
