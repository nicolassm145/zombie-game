using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Shotgun : Weapon
{
    [SerializeField] private int bulletsPerShot = 5;
    [SerializeField] private float spreadAngle = 30f;

    protected override void PerformFire()
    {
        audioSource.pitch = 1.0f;
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(fireSound);

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

        for (int i = 0; i < bulletsPerShot; i++)
        {
            // Calcula o ângulo de cada projétil dentro do cone
            float angleOffset = Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, (float)i / (bulletsPerShot - 1));
            Quaternion rotation = Quaternion.Euler(0, 0, angleOffset); // Rotação em z para cada projétil

            // Aplica a rotação no vetor direção
            Vector2 rotatedDirection = rotation * direction;

            // Cria a bala e ajusta sua direção
            GameObject newBullet = Instantiate(bullet, SpawnerBulletPos.transform.position, Quaternion.identity);
            newBullet.transform.up = rotatedDirection; // Define a direção da bala
            newBullet.GetComponent<Bullet>().Damage = damage;
        }
    }

    protected override IEnumerator PerformReload()
    {
        // Conta o tempo baseado no número de balas
        int bulletsToReload = 0;
        bulletsToReload = currentAmmo + currentMagazineAmmo < maxMagazineAmmo
            ? currentAmmo
            : maxMagazineAmmo - currentMagazineAmmo;
        
        print(bulletsToReload);

        float elapsed = 0f;
        float elapsedBullet = 0f; // Tempo para carregar uma bala
        
        float totalTime = reloadTime * bulletsToReload;
        
        while (elapsed < totalTime)
        {
            if (!IsReloading)
            {
                // Sai do reload sem completar
                audioSource.volume = 0;
                slideBarObject.SetActive(false); // Esconde a barra
                yield break;
            }

            if (elapsedBullet >= reloadTime)
            {
                elapsedBullet -= reloadTime;
                currentAmmo--;
                currentMagazineAmmo++;
            }
            
            elapsed += Time.deltaTime;
            elapsedBullet += Time.deltaTime;
            reloadBar.value = elapsed / totalTime; // Atualiza a barra de progresso
            yield return null; // Espera o próximo frame
        }
        
        currentAmmo--;
        currentMagazineAmmo++;
    }
}
