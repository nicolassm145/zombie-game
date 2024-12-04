using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : Weapon
{
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

        // Cria a bala e ajusta sua direção
        GameObject newBullet = Instantiate(bullet, SpawnerBulletPos.transform.position, Quaternion.identity);
        newBullet.transform.up = direction;
    }

    protected override IEnumerator PerformReload()
    {
        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            if (!IsReloading)
            {
                // Sai do reload sem completar
                audioSource.volume = 0;
                slideBarObject.SetActive(false); // Esconde a barra
                yield break;
            }
            
            elapsed += Time.deltaTime;
            reloadBar.value = elapsed / reloadTime; // Atualiza a barra de progresso
            yield return null; // Espera o próximo frame
        }
        
        // Completa o reload
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
    }
}
