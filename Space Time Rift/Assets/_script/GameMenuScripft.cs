using UnityEngine.UI;
using UnityEngine;

public class GameMenuScripft : MonoBehaviour
{
    [SerializeField] GameObject gameMenu;

    private void Awake()
    {
        gameMenu = GameObject.Find("GameMenu");
    }
    private void Start()
    {
        gameMenu.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { OnPlayBtn(); });
        gameMenu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { OnQuitBtn(); });
    }

    private void OnPlayBtn()
    {
        gameMenu.SetActive(false);
    }

    private void OnQuitBtn()
    {
        Application.Quit();
    }
}
