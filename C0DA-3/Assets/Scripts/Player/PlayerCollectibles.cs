using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    [Header("Collectibles")]
    [SerializeField] private int currentCollectibles = 0;

    public int CurrentCollectibles => currentCollectibles;

    public void AddCollectible(int amount)
    {
        currentCollectibles += amount;
        Debug.Log("Núcleos actuales: " + currentCollectibles);
    }
}