using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    // Bullet
    public GameObject bullet;
    public GameObject SpawnerBulletPos { get; set; }

    // Recarregar
    [SerializeField] protected GameObject slideBarObject;
    protected Slider reloadBar;
    public bool IsReloading { get; set; }
    public float reloadTime;

    // Munição
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int maxMagazineAmmo;
    protected int currentAmmo;
    protected int currentMagazineAmmo;
    protected TextMeshProUGUI ammoText;
    protected TextMeshProUGUI warningText;

    // Sons
    [SerializeField] protected AudioClip fireSound;
    [SerializeField] protected AudioClip reloadSound;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        reloadBar = slideBarObject.GetComponent<Slider>();
        ammoText = GameObject.FindWithTag("AmmoUI")?.GetComponent<TextMeshProUGUI>();
        warningText = GameObject.FindWithTag("Warning")?.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        if (currentMagazineAmmo == 0) return;
        IsReloading = false;
        PerformFire();
        currentMagazineAmmo--;
        UpdateAmmoUI();
        CheckAmmo();
    }

    public IEnumerator IEReload()
    {
        if (currentMagazineAmmo == maxMagazineAmmo || currentAmmo == 0) yield break;
        IsReloading = true;
        slideBarObject.SetActive(true);

        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(reloadSound);

        yield return PerformReload();

        slideBarObject.SetActive(false);
        IsReloading = false;
        UpdateAmmoUI();
        CheckAmmo();
    }

    public void BuyAmmo()
    {
        currentAmmo = maxAmmo;
        currentMagazineAmmo = maxMagazineAmmo;
        UpdateAmmoUI();
        CheckAmmo();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText)
            ammoText.text = $"{currentMagazineAmmo}/{currentAmmo}";
    }

    private void CheckAmmo()
    {
        if (warningText == null) return;
        string text = warningText.text;
        string reloadWarning = "Pressione [R] para recarregar\n";
        string ammoWarning = "Munição acabando\n";

        if (currentMagazineAmmo <= maxMagazineAmmo * 0.3f && !text.Contains(reloadWarning))
            text += reloadWarning;
        else if (currentMagazineAmmo > maxMagazineAmmo * 0.3f)
            text = text.Replace(reloadWarning, "");

        if (currentAmmo <= maxMagazineAmmo && !text.Contains(ammoWarning))
            text += ammoWarning;
        else if (currentAmmo > maxMagazineAmmo)
            text = text.Replace(ammoWarning, "");

        warningText.text = text;
    }

    // Métodos abstratos para customizações
    protected abstract void PerformFire();
    protected abstract IEnumerator PerformReload();
}