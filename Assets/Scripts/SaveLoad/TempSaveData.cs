[System.Serializable]
public class TempSaveData
{
    public int playerCollectedCoins;
    public float playerHealth;

    public TempSaveData(int coins = 0, float health = 0f)
    {
        playerCollectedCoins = coins;
        playerHealth = health;
    }
}