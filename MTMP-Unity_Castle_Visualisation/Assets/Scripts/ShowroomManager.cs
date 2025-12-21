using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowroomManager : MonoBehaviour
{
    public CameraController cc;
    public UiManager ui;
    public GameObject manualShowroom;
    public GameObject automaticShowroom;
    public GameObject mainMenu;
    public GameObject infoPanel;
    private bool isAutoPlaying = false;
    public Transform[] autoTargets;
    public GameObject[] infoPanels;
    public float autoDelay = 7f;
    private int autoIndex = 0;
    private float autoTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        manualShowroom.SetActive(false);
        automaticShowroom.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SelectManual()
    {
        manualShowroom.SetActive(true);
        automaticShowroom.SetActive(false);
        mainMenu.SetActive(false);
        infoPanel.SetActive(false);
        isAutoPlaying = false;
    }

    public void SelectAutomatic()
    {
        manualShowroom.SetActive(false);
        automaticShowroom.SetActive(true);
        mainMenu.SetActive(false);
        infoPanel.SetActive(false);
        isAutoPlaying = true;
        AutomaticSequence();
    }

    // Update is called once per frame
    void Update()
    {
        AutomaticSequence();
    }

    public void AutomaticSequence()
    {
        if (!isAutoPlaying) return;

        autoTimer += Time.deltaTime;
        infoPanels[autoIndex].SetActive(true);
        if (autoIndex == 0)
        {
            autoDelay = 15f;
            ui.RotationShip1(true);
        }
        if (autoIndex == 4)
        {
            autoDelay = 15f;
            ui.RotationShip2(true);
        }
        if (autoTimer >= autoDelay)
        {
            infoPanels[autoIndex].SetActive(false);
            if (autoIndex == 0)
            {
                ui.RotationShip1(false);
                autoDelay = 7f;
            }
            if (autoIndex == 4)
            {
                ui.RotationShip2(false);
                autoDelay = 7f;
            }
            autoTimer = 0f;
            autoIndex++;
            if (autoIndex >= autoTargets.Length)
            {
                autoIndex = 0;
                autoTimer = 0f;
                ui.RotationShip1(false);
                ui.RotationShip2(false);
                cc.StartCameraMove(autoTargets[0].gameObject);
                isAutoPlaying = false;
                mainMenu.SetActive(true);
                automaticShowroom.SetActive(false);
                return;
            }

            cc.StartCameraMove(autoTargets[autoIndex].gameObject);
        }
    }

    public void HaltAutomatic()
    {
        isAutoPlaying = false;
        autoIndex = 0;
        autoTimer = 0f;
        autoDelay = 7f;
        ui.RotationShip1(false);
        ui.RotationShip2(false);
        cc.StartCameraMove(autoTargets[0].gameObject);
        mainMenu.SetActive(true);
        automaticShowroom.SetActive(false);
    }

    public void BackToMenu()
    {
        ui.RotationShip1(false);
        ui.RotationShip2(false);
        cc.ManageCameraLock(false);
        cc.StartCameraMove(autoTargets[0].gameObject);
        mainMenu.SetActive(true);
        manualShowroom.SetActive(false);
    }

    public void QuitShowroom()
    {
        Application.Quit();
    }

    public void ShowInfo(bool status)
    {
        infoPanel.SetActive(status);
    }
}
