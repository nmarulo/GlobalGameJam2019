﻿using UnityEngine;

public class PigController : MonoBehaviour
{
    private int wheat, wood, stone;
    public Material currentMaterial;
    private JoystickController joystickController;

    public void Start()
    {
        this.joystickController = GetComponent<JoystickController>();
    }

    private void Awake()
    {
        ResetMaterials();

    }

    private void Update()
    {

    }


    private bool UseMaterial()
    {
        bool used = false;
        if (currentMaterial == Material.Wheat && wheat > 0)
        {
            // TODO reproduce use wheat sound
            wheat--;
            used = true;
        }
        else if (currentMaterial == Material.Wood && wood > 0)
        {
            // TODO reproduce use wood sound
            wood--;
            used = true;
        }
        else if (currentMaterial == Material.Stone && stone > 0)
        {
            // TODO reproduce use stone sound
            stone--;
            used = true;
        }
        if (!used)
        {
            // TODO sound pig without materials?
        }
        return used;
    }

    public void ChangeMaterial(Material newMaterial)
    {
        currentMaterial = newMaterial;
        ResetMaterials();
        switch (currentMaterial)
        {
            case Material.Wheat:
                // TODO reproduce get wheat sound
                wheat = GameConfiguration.WHEATCAPACITY;
                break;
            case Material.Wood:
                // TODO reproduce get wood sound
                wood = GameConfiguration.WOODCAPACITY;
                break;
            case Material.Stone:
                // TODO reproduce get stone sound
                stone = GameConfiguration.STONECAPACITY;
                break;
        }
    }

    public bool IsRepairingWall()
    {
        return joystickController.IsPressAnyButton();
        //return Input.GetKeyDown(KeyCode.R);
    }

    public bool IsGettingResource()
    {
        return joystickController.IsPressAnyButton();
        //return Input.GetKeyDown(KeyCode.R);
    }

    public Material GetCurrentMaterial()
    {
        return currentMaterial;
    }

    private void ResetMaterials()
    {
        wheat = 0;
        wood = 0;
        stone = 0;
    }
}
