using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform playerPrefab;

    public void SpawnPlayer (Vector3 position, Quaternion rotation)
    {
        Transform player = Instantiate(playerPrefab, position, rotation);

        AudioManager.InstantiateAudioSource(player.position, "Spooky Ambience", player);
    }
}
