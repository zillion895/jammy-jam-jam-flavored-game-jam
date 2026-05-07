using Godot;
using System;

public partial class startButton : Button
{
	public Control mainMenu;
	public override void _Ready()
	{
		mainMenu = GetParent<Control>()?.GetParent<Control>();
	}
	public async override void _Pressed()
	{
		var tween = CreateTween();
		tween.TweenProperty(mainMenu, "modulate:a", 0, 0.5f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
		await ToSignal(tween, "finished");
		GetTree().ChangeSceneToFile("res://scenes/levels/mainLevel.tscn");
	}
}	
