[System.Serializable]
public class GameModes
{
    public string Name;

    public bool PlayerTakesDamage;

    public bool BuildingsCostFlotsam;

    public GameModes()
    {
        SetSurvivalMode();
    }

    public void SetCreativeMode()
    {
        Name = "Creative";
        PlayerTakesDamage = false;
        BuildingsCostFlotsam = false;
    }

    public void SetSurvivalMode()
    {
        Name = "Survival";
        PlayerTakesDamage = true;
        BuildingsCostFlotsam = true;
    }
}