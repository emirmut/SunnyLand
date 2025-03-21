using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OnSceneSwitchCoroutine(string sceneName) {
        Time.timeScale = 1;
        animator.SetTrigger("sceneTransitionAnimationStart");
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        
    }

    public void OnSceneSwitch(string sceneName) {
        StartCoroutine(OnSceneSwitchCoroutine(sceneName));
    }
}
