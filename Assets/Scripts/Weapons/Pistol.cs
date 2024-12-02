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
    private TextMeshProUGUI _warningText;

    private void Start()
    {
        _reloadBar = slideBarObject.GetComponent<Slider>();
        _ammoText = GameObject.FindWithTag("AmmoUI")?.GetComponent<TextMeshProUGUI>();
        _warningText = GameObject.FindWithTag("Warning")?.GetComponent<TextMeshProUGUI>();
    }

    public void Fire()
    {
        if (_currentMagazineAmmo == 0) return;

        IsReloading = false; // Cancela o reload
    
        Vector2 direction;

        // Detecta se um controle está conectado e o analógico está sendo usado
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && gamepad.rightStick.ReadValue().magnitude > 0.1f)
        {
            // Usa o analógico direito para calcular a direção
            direction = gamepad.rightStick.ReadValue();
        }
        else
        {
            // Usa o mouse para calcular a direção
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
            direction = new Vector2(
                mousePos.x - SpawnerBulletPos.transform.position.x,
                mousePos.y - SpawnerBulletPos.transform.position.y
            );
        }

        direction.Normalize(); // Normaliza a direção para garantir que o vetor seja unitário

        // Cria a bala e ajusta sua direção
        GameObject newBullet = Instantiate(bullet, SpawnerBulletPos.transform.position, Quaternion.identity);
        newBullet.transform.up = direction;

        _currentMagazineAmmo--;
        UpdateAmmoUI();
        CheckAmmo();
    }

    public IEnumerator IEReload()
    {
        if (_currentMagazineAmmo == maxMagazineAmmo || _currentAmmo == 0) yield break;   // Não recarrega se estiver com munição cheia
        
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
        CheckAmmo();
    }
    
    public void BuyAmmo()
    {
        _currentAmmo = maxAmmo;
        _currentMagazineAmmo = maxMagazineAmmo;
        UpdateAmmoUI();
        CheckAmmo();
    }

    private void UpdateAmmoUI()
    {
        // Atualize a UI de munição se existir
        if (_ammoText)
        {
            _ammoText.text = $"{_currentMagazineAmmo}/{_currentAmmo}";
        }
    }

    private void CheckAmmo()
    {
        if (_warningText == null) return;

        string text = _warningText.text;

        // Gerenciar a mensagem de recarregar
        string reloadWarning = "Pressione [R] para recarregar\n";
        if (_currentMagazineAmmo <= maxMagazineAmmo * 0.3f)
        {
            if (!text.Contains(reloadWarning))
            {
                text += reloadWarning; // Adiciona a mensagem de recarregar
            }
        }
        else
        {
            int reloadStartIndex = text.IndexOf(reloadWarning);
            if (reloadStartIndex != -1)
            {
                text = text.Remove(reloadStartIndex, reloadWarning.Length); // Remove a mensagem de recarregar
            }
        }

        // Gerenciar a mensagem de munição acabando
        string ammoWarning = "Munição acabando\n";
        if (_currentAmmo <= maxMagazineAmmo)
        {
            if (!text.Contains(ammoWarning))
            {
                text += ammoWarning; // Adiciona a mensagem de munição acabando
            }
        }
        else
        {
            int ammoStartIndex = text.IndexOf(ammoWarning);
            if (ammoStartIndex != -1)
            {
                text = text.Remove(ammoStartIndex, ammoWarning.Length); // Remove a mensagem de munição acabando
            }
        }

        _warningText.text = text;
    }
}
