using System.Collections;
using UnityEngine;

public enum ItemType { Money, Ammo }

public class ItemDrop : MonoBehaviour
{
    public ItemType itemType; // Tipo do item
    public int value;         // Valor ou quantidade do item
    public AudioClip moneyPickupSound;
    public AudioClip genericPickupSound;
    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Money:
                        player.AddMoney(value);
                        audioSource.PlayOneShot(moneyPickupSound); // Toca som de dinheiro
                        break;
                    case ItemType.Ammo:
                        // Outros tipos compartilham o mesmo som
                        if (itemType == ItemType.Ammo)
                            player.Weapon.BuyAmmo();

                        audioSource.PlayOneShot(genericPickupSound); // Toca som genérico
                        break;
                }

                // Evita destruir o objeto até que o som termine
                StartCoroutine(DestroyAfterSound());
            }
        }
    }
    
    IEnumerator DestroyAfterSound()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();

        // Desabilita renderização e colisão para o item "sumir"
        if (sprite != null) sprite.enabled = false;
        if (col != null) col.enabled = false;
        
        yield return new WaitForSeconds(0.5f); 
        Destroy(gameObject);
    }


}