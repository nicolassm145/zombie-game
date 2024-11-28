using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    // Bullet
    public GameObject bullet;
    public GameObject SpawnerBulletPos { get; set; }
    
    // Recarregar
    [SerializeField] private GameObject slideBarObject;
    private Slider _reloadBar;
    public bool IsReloading { get; set; }
    public float reloadTime = 3f;

    // Munição
    [SerializeField] private int maxAmmo;
    [SerializeField] private int maxMagazineAmmo;
    private int _currentAmmo;
    private int _currentMagazineAmmo;
    private TextMeshProUGUI _ammoText;

    private void Start()
    {
        _reloadBar = slideBarObject.GetComponent<Slider>();
        _ammoText = GameObject.FindWithTag("AmmoUI")?.GetComponent<TextMeshProUGUI>();
    }

    public void Fire()
    {
        if (_currentMagazineAmmo == 0) return;

        IsReloading = false; // Cancela o reload
        
        // Obtém a posição do mouse na tela
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

        // Calcula a direção do tiro
        Vector2 direction = new Vector2(
            mousePos.x - SpawnerBulletPos.transform.position.x,
            mousePos.y - SpawnerBulletPos.transform.position.y
        );
        direction.Normalize();

        // Cria a bala e ajusta sua direção
        GameObject newBullet = Instantiate(bullet, SpawnerBulletPos.transform.position, Quaternion.identity);
        newBullet.transform.up = direction;

        _currentMagazineAmmo--;
        UpdateAmmoUI();
    }

    public IEnumerator IEReload()
    {
        IsReloading = true;
        slideBarObject.SetActive(true); // Ativa a barra de progresso

        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            if (!IsReloading)
            {
                // Sai do reload sem completar
                slideBarObject.SetActive(false); // Esconde a barra
                yield break;
            }
            
            elapsed += Time.deltaTime;
            _reloadBar.value = elapsed / reloadTime; // Atualiza a barra de progresso
            yield return null; // Espera o próximo frame
        }

        // Completa o reload
        if (_currentAmmo + _currentMagazineAmmo < maxMagazineAmmo)
        {
            _currentMagazineAmmo += _currentAmmo;
            _currentAmmo = 0;
        }
        else
        {
            _currentAmmo = _currentAmmo - maxMagazineAmmo + _currentMagazineAmmo;
            _currentMagazineAmmo = maxMagazineAmmo;
        }

        slideBarObject.SetActive(false); // Oculta a barra após o recarregamento
        IsReloading = false;

        UpdateAmmoUI();
    }
    
    public void BuyAmmo()
    {
        _currentAmmo = maxAmmo;
        _currentMagazineAmmo = maxMagazineAmmo;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        // Atualize a UI de munição se existir
        if (_ammoText)
        {
            _ammoText.text = $"{_currentMagazineAmmo}/{_currentAmmo}";
        }
    }
}
