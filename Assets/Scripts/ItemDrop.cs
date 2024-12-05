using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum ItemType { Money, Ammo }

public class ItemDrop : MonoBehaviour
{
    public ItemType itemType; // Tipo do item
    public int value;         // Valor ou quantidade do item
    public AudioClip moneyPickupSound;
    public AudioClip genericPickupSound;
    private AudioSource audioSource;
    
    private TextMeshProUGUI itemInfoText;
    [SerializeField] float displayDuration = 2f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameObject textObject = GameObject.FindWithTag("Warning");
        itemInfoText = textObject.GetComponent<TextMeshProUGUI>(); 
        itemInfoText.text = ""; // Certifique-se de que começa vazio
        
        
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
                        player.Weapon.BuyAmmo();
                        ShowItemInfo("Você coletou munição!");
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

    void ShowItemInfo(string message)
    {
        if (itemInfoText != null)
        {
            StopAllCoroutines(); // Interrompe qualquer coroutine anterior
            itemInfoText.text = message + "\n"; // Define a nova mensagem
            StartCoroutine(ClearItemInfoAfterDelay());
        }
        else
        {
            Debug.LogError("itemInfoText não está configurado. Verifique no Unity.");
        }
    }

    IEnumerator ClearItemInfoAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        itemInfoText.text = ""; 
          
    }
}
