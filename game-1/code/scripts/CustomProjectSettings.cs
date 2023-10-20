namespace FlappyDragon;

using Godot;

public static class CustomProjectSettings
{
    public static readonly float MovementSpeed = ProjectSettings.GetSetting("flappy_dragon/movement/move_speed").AsSingle();
}