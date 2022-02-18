using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject player;
    public GameObject playersWeapon;
    public SpriteManager spriteManager;
    public WeaponController playerWeaponController;

    public WeaponController.WeaponType[] inventory;
    public GameObject[] slots;
    public GameObject toolbarSelection;

    public int slotSelected;
    public bool slotChanged;

    void Start()
    {
        spriteManager = GameObject.FindGameObjectWithTag("GameManager").transform.GetChild(0).GetComponent<SpriteManager>();
        inventory = new WeaponController.WeaponType[3] { WeaponController.WeaponType.ElectronPistol, WeaponController.WeaponType.NONE, WeaponController.WeaponType.NONE };
        slots = new GameObject[3] { gameObject.transform.GetChild(0).gameObject, gameObject.transform.GetChild(1).gameObject, gameObject.transform.GetChild(2).gameObject };

        player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon = inventory[0];
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerWeaponController.GetComponent<WeaponController>().WeaponSprites[player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon];

        slotSelected = 1;
        slotChanged = false;

        SetToolbarSprite();
    }

    void Update()
    {
        if (slotChanged)
        {
            UpdateSlot();
        }
    }

    public void UpdateSlot()
    {
        if (slotSelected == 1)
        {
            toolbarSelection.transform.localPosition = new Vector3(-60, 0, 0);
            player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon = inventory[0];
        }

        else if (slotSelected == 2)
        {
            toolbarSelection.transform.localPosition = new Vector3(0, 0, 0);
            player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon = inventory[1];
        }

        else if (slotSelected == 3)
        {
            toolbarSelection.transform.localPosition = new Vector3(60, 0, 0);
            player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon = inventory[2];
        }
        
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerWeaponController.GetComponent<WeaponController>().WeaponSprites[player.transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon];
        
        SetToolbarSprite();
        slotChanged = false;
    }

    public void SetToolbarSprite()
    {
        for (int i = 0; i < 3; i++)
        {
            if (inventory[i] == WeaponController.WeaponType.NONE)
            {
                slots[i].GetComponent<Image>().sprite = spriteManager.toolbarNONE;
            }

            else if (inventory[i] == WeaponController.WeaponType.ElectronPistol)
            {
                slots[i].GetComponent<Image>().sprite = spriteManager.toolbarElectronPistol;
            }

            else if (inventory[i] == WeaponController.WeaponType.EntanglementGun)
            {
                slots[i].GetComponent<Image>().sprite = spriteManager.toolbarEntanglementGun;
            }

            else if (inventory[i] == WeaponController.WeaponType.PhotonGun)
            {
                slots[i].GetComponent<Image>().sprite = spriteManager.toolbarPhotonGun;
            }
        }
    }
}
