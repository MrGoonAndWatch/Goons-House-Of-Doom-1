public static class GameConstants
{
    public const double GreenMedicineHp = 69.420;

    public static class Controls
    {
        public const string Aim = "Aim";
        public const string VerticalMovement = "Vertical";
        public const string HorizontalMovement = "Horizontal";
        public const string Action = "Action";
        public const string Run = "Run";
    }
}

public enum HealthStatus
{
    None = 0,
    Dead,
    Special,
    SpeedyBoi,
    BadTummyAche,
    TummyAche,
    Healthy
}

public enum KeyType
{
    None = 0,
    WardrobeKey = 1,
}
