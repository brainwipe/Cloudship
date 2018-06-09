[System.Serializable]
public class GameModes
{
    public string Name;

    public bool PlayerTakesDamage;

    public GameModes()
    {
        SetSurvivalMode();
    }

    public void SetCreativeMode()
    {
        Name = "Creative";
        PlayerTakesDamage = false;
    }

    public void SetSurvivalMode()
    {
        Name = "Survival";
        PlayerTakesDamage = true;
    }
}