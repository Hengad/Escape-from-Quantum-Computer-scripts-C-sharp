using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public GameObject playersWeapon;

    public GameObject sceneTransitionFade;
    public Text sceneNumberText;

    public GameObject weaponInfoCanvas;
    public Text weaponNameText;
    public Text weaponInfoText;

    private void Update()
    {
        if(playersWeapon.GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.NONE)
        {
            weaponInfoCanvas.SetActive(false);
        }

        else
        {
            weaponInfoCanvas.SetActive(true);
            if(playersWeapon.GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.ElectronPistol)
            {
                weaponNameText.text = "Electron Pistol";
                weaponInfoText.text = "Starting gun. Works well against\nmetallic enemies (white).";
            }

            else if (playersWeapon.GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.EntanglementGun)
            {
                weaponNameText.text = "Entanglement Gun";
                weaponInfoText.text = "Used to entangle multiple enemies.\nAttacking an entangled enemy affects\nall the other entangled ones too.";
            }

            else if (playersWeapon.GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.PhotonGun)
            {
                weaponNameText.text = "Photon Gun";
                weaponInfoText.text = "High fire rate photons shooting gun.\nWorks well against graphene (black)\nwhich absorbs the shot photons.";
            }
        }
    }

    public void SceneTransition(int number)
    {
        //Time.timeScale = 0.0f; this is done in the animation
        sceneTransitionFade.SetActive(false);
        sceneTransitionFade.SetActive(true);

        sceneNumberText.text = (number - 1).ToString();
    }
}
