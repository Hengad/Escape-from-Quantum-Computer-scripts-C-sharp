using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    public event EventHandler<WeaponController.OnShootEventArgs> OnShoot;

    public GameObject deathCanvas;
    public Image healthFill;
    public Image energyFill;
    public InventoryController inventory;
    SpriteManager spriteManager;
    SoundController soundController;

    public int maxHealth;
    public int health;
    public int maxEnergy;
    public int energy;
    public float speed;

    private float horizontal;
    private float vertical;
    private float healthBarWidth;
    private float energyBarWidth;

    SpriteRenderer sprite;
    private Color originalColor;
    private Rigidbody2D rb;

    public void OnHit()
    {
        soundController.playerHitSound.Play();

        StartCoroutine(OnHitTimer());

        this.health -= 1; //enemy's damage on player always 1, no matter what weapon
        float newHealthBarWidth = health * healthBarWidth / maxHealth;
        healthFill.rectTransform.sizeDelta = new Vector2(newHealthBarWidth, healthFill.rectTransform.rect.height);
        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator OnHitTimer()
    {
        sprite.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        sprite.color = originalColor;
    }

    private void Die()
    {
        deathCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void Start()
    {
        spriteManager = GameObject.FindGameObjectWithTag("GameManager").transform.GetChild(0).GetComponent<SpriteManager>();
        soundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        maxHealth = 3;
        health = maxHealth;
        maxEnergy = 100;
        energy = maxEnergy;
        speed = 3.5f;
        healthBarWidth = healthFill.rectTransform.rect.width;
        energyBarWidth = energyFill.rectTransform.rect.width;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateShooting();
        UpdateToolbar();
    }

    private void UpdateToolbar()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.slotSelected = 1;
            inventory.slotChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.slotSelected = 2;
            inventory.slotChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.slotSelected = 3;
            inventory.slotChanged = true;
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 dropPosition = transform.position + new Vector3(mouseOnScreen.x - positionOnScreen.x, mouseOnScreen.y - positionOnScreen.y, 0.0f).normalized * 1.2f;
            Transform droppedItem = Instantiate(gameManager.droppedWeapon, dropPosition, Quaternion.Euler(new Vector3(0f, 0f, 0.0f)));
            droppedItem.GetComponent<DropController>().dropType = inventory.inventory[inventory.slotSelected-1];
            inventory.inventory[inventory.slotSelected - 1] = WeaponController.WeaponType.NONE;

            if (transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.ElectronPistol)
            {
                droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.toolbarElectronPistol;
            }

            else if (transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.EntanglementGun)
            {
                droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.toolbarEntanglementGun;
            }

            else if (transform.GetChild(0).GetComponent<WeaponController>().selectedWeapon == WeaponController.WeaponType.PhotonGun)
            {
                droppedItem.GetComponent<SpriteRenderer>().sprite = spriteManager.toolbarPhotonGun;
            }

            inventory.UpdateSlot();
        }
    }

    private void UpdateShooting()
    {
        if (Input.GetMouseButton(0))
        {
            OnShoot?.Invoke(this, new WeaponController.OnShootEventArgs
            {
                shootStartPos = transform.position,
                shootEndPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition),
                shotByPlayer = true
            });

            float newEnergyBarWidth = energy * energyBarWidth / maxEnergy;
            energyFill.rectTransform.sizeDelta = new Vector2(newEnergyBarWidth, energyFill.rectTransform.rect.height);
        }
    }

    private void UpdateMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            gameManager.ChangeLevel(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ItemDrop")
        {
            if (collision.gameObject.GetComponent<DropController>().dropType == WeaponController.WeaponType.Heart)
            {
                if(health < maxHealth)
                {
                    this.health += 1;
                    float newHealthBarWidth = health * healthBarWidth / maxHealth;
                    healthFill.rectTransform.sizeDelta = new Vector2(newHealthBarWidth, healthFill.rectTransform.rect.height);
                }

                Destroy(collision.gameObject);
            }

            else
            {
                for (int i = 0; i < inventory.inventory.Length; i++)
                {
                    if (inventory.inventory[i] == WeaponController.WeaponType.NONE)
                    {
                        inventory.inventory[i] = collision.gameObject.GetComponent<DropController>().dropType;
                        inventory.UpdateSlot();

                        Destroy(collision.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gate")
            speed = 3.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            gameManager.playerTriesToEnterGate = true;
            gameManager.ChangeLevel(transform.position);
        }
    }

}
