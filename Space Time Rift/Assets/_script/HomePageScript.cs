using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomePageScript : MonoBehaviour
{
    [SerializeField] GameObject homePage;
    [SerializeField] GameObject TimeLineBtns;


    private void Awake()
    {
        homePage = GameObject.Find("HomePage");
        TimeLineBtns = GameObject.Find("TimeLineBtns");
    }

    void Start()
    {
        TimeLineBtns.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
        TimeLineBtns.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
        TimeLineBtns.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
        TimeLineBtns.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
        TimeLineBtns.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        homePage.transform.GetChild(2).gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            homePage.transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = progressValue;

            yield return null;
        }
    }
}
