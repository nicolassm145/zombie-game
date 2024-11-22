using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : MonoBehaviour
{
    public GameObject bullet, spawnerBulletPos;

    // Munição
    [SerializeField] private int maxAmmo;
    [SerializeField] private int maxMagazineAmmo;
    public int currentAmmo { get; private set; }
    public int currentMagazineAmmo { get; private set; }

    public void Fire()
    {
        if (currentMagazineAmmo == 0) return;
        
        // Obtém a posição do mouse na tela
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

        // Calcula a direção do tiro
        Vector2 direction = new Vector2(
            mousePos.x - spawnerBulletPos.transform.position.x,
            mousePos.y - spawnerBulletPos.transform.position.y
        );
        direction.Normalize();

        // Cria a bala e ajusta sua direção
        GameObject newBullet = Instantiate(bullet, spawnerBulletPos.transform.position, Quaternion.identity);
        newBullet.transform.up = direction;

        currentMagazineAmmo--;
        
        UpdateAmmoUI();
    }

    public void Reload()
    {
        
        if (currentAmmo + currentMagazineAmmo < maxMagazineAmmo)
        {
            currentMagazineAmmo += currentAmmo;
            currentAmmo = 0;
        }
        else
        {
            currentAmmo = currentAmmo - maxMagazineAmmo + currentMagazineAmmo;
            currentMagazineAmmo = maxMagazineAmmo;
        }

        UpdateAmmoUI();
    }
    
    public void BuyAmmo()
    {
        currentAmmo = maxAmmo;
        currentMagazineAmmo = maxMagazineAmmo;
        UpdateAmmoUI();
    }
    
    void UpdateAmmoUI()
    {
        // Atualize a UI de munição se existir
        TextMeshProUGUI ammoText = GameObject.FindWithTag("AmmoUI")?.GetComponent<TextMeshProUGUI>();
        if (ammoText != null)
        {
            ammoText.text = $"{currentMagazineAmmo}/{currentAmmo}";
        }
    }
}
