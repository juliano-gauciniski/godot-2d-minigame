using Godot;
using System;

public partial class Sword : Node2D
{
	private AnimationPlayer _swordAnimationPlayer;
	private Hitbox _swordHitbox;

	private const int SWORD_OFFSET_Y_POS = 200;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get the _swordAnimationPlayer
		_swordAnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		//Get the _swordHitbox
		_swordHitbox = GetNode<Hitbox>("Parts/Hitbox");
		
	}
	
	//Override the _Input method
	public override void _Input(InputEvent @event)
	{
		//Check if the event is the input mouse motion event
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			//Set the Position of the sword to be the global position of the mouse
			Position = eventMouseMotion.GlobalPosition;
		}
		
		//if the left mouse button is pressed, play the sword attack animation
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed())
		{
			if (eventMouseButton.ButtonIndex == (MouseButton)MouseButton.Left)
			{
				//Set the attack from vector
				_swordHitbox.SetAttackFromVector(GlobalPosition - new Vector2(0, SWORD_OFFSET_Y_POS));
				_swordAnimationPlayer.Play("SwordAttack");
			}
		}
	}
}
