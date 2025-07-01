using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public int maxHealth = 100;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
    }

    [ServerRpc]
    public void TakeDamageServerRpc(int damage)
    {
        if (currentHealth.Value <= 0) return;

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // L�gica de morte (pode incluir respawn, desativa��o etc.)
        gameObject.SetActive(false);
    }
}

