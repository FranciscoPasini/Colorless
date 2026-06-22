using System;
using System.Collections;
using UnityEngine;

public class TabletUI : MonoBehaviour
{
    [SerializeField] private GameObject powerOff;
    [SerializeField] private GameObject powerOn;
    [SerializeField] private GameObject unlockScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject workSpaceScreen;

    [SerializeField] private Animator animator;

    private Action onPowerOnAction;

    private void Awake()
    {
        onPowerOnAction = () => StartCoroutine(PlayFadeInAndTransition());
    }

    public void OpenPanel(GameObject panel)
    {
        powerOff.SetActive(false);
        powerOn.SetActive(false);
        unlockScreen.SetActive(false);
        menuScreen.SetActive(false);
        workSpaceScreen.SetActive(false);

        panel.SetActive(true);

        if (panel == powerOn)
        {
            StartCoroutine(ExecuteActionNextFrame(onPowerOnAction));
        }
    }

    private IEnumerator ExecuteActionNextFrame(Action actionToExecute)
    {
        yield return null;
        actionToExecute?.Invoke();
    }

    private IEnumerator PlayFadeInAndTransition()
    {
        animator.Play("FadeIn");

        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        OpenPanel(unlockScreen);
    }
}