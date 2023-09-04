public class SoilData
{
    public bool HasPlant;
    public bool Watered;

    public PlantState PlantState;
}

public enum PlantState
{
    seed = 0,
    smallPlant,
    ripe
}