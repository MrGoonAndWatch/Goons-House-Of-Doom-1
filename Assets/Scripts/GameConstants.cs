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
        public const string Menu = "Menu";
        public const string Pause = "Pause";
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
    BigKey = 2,
    WestHallway,
    DiningRoom,
}

public enum GlobalEvent
{
    None = 0,
    LabUnlocked = 1,
    SelfDestructEngaged = 2,
    TookHandgunWestHallway = 3,
    TookStarlightTypewriter = 4,
    TookSkullSpiderTypewriter = 5,
}

public enum DoorLoadType
{
    None = 0,
    WoodDoor1 = 1,
}

public static class AnimationNames
{
    public static class Enemy
    {
        public const string Attack = "Attack";
    }
}

public static class AnimationVariables
{
    public static class Player
    {
        public const string Walking = "Walking";
        public const string Running = "Running";
        public const string Dead = "Dead";
        public const string Bitten = "Bitten";
    }

    public static class Enemy
    {
        public const string Moving = "Moving";
        public const string Attacking = "Attacking";
    }
}

public static class AnimationLayers
{
    public const string BaseLayer = "Base Layer";

    public static class Player
    {
        public const string EquipLayer = "Equip Layer";
    }
}