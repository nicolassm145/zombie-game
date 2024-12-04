using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public enum ItemType { Money, Ammo }
    public ItemType itemType;
    public int value; // Valor do item (quantidade de dinheiro ou munição)
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (itemType == ItemType.Money)
                {
                    player.AddMoney(value); // Adiciona dinheiro ao jogador
                }
                else if (itemType == ItemType.Ammo)
                {
                    player.Weapon.BuyAmmo(); // Adiciona munição
                }
            }
            Destroy(gameObject); // Remove o item do jogo
        }
    }
}