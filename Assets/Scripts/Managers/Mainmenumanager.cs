using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject lightScene;
    public GameObject house;
    public GameObject canvas1;
    public GameObject daymanager;
    public GameObject clock;
    public GameObject activitymanager;
    public GameObject canvas2;

    public void PlayButton()
    {
        lightScene.SetActive(true);
        house.SetActive(true);
        canvas1.SetActive(true);
        daymanager.SetActive(true);
        clock.SetActive(true);
        activitymanager.SetActive(true);

        canvas2.SetActive(false);
    }
}
