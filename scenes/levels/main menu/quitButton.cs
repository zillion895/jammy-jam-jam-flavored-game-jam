using Godot;
using System;

public partial class quitButton : Button
{
	
	public override void _Pressed()
	{
		GetTree().Quit();
	}
}
