using Godot;
using System;

public partial class Hurtbox : Area2D
{
	private LayersAndMasks _layersAndMasks;
	private Node _mainNode;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get the LayerAndMasks singleton
		//_layersAndMasks = GetNode<LayersAndMasks>("/root/LayersAndMasks");
		//Set the collision layer to 0 = no layer
		//CollisionLayer = 0;
		//Get the hitbox collision layer by name
		//CollisionMask = _layersAndMasks.GetCollisionLayerByName("World");
		
		//Get the root node
		_mainNode = GetTree().Root.GetNode("Main");
		GD.Print("ROOT NODE: " + _mainNode.Name);
		
		//Create a signal for area_entered
		Callable callable = new Callable(this, MethodName.OnAreaEntered);
		Connect("area_entered", callable);
		
	}

	private void OnAreaEntered(Area2D area2D)
	{
		if (area2D != null)
		{
			//1 - World Edge
			if (area2D.GetCollisionLayerValue(1))
			{
				_mainNode.EmitSignal("GameOver");
			}
			//4 - Hitbox
			if (area2D.GetCollisionLayerValue(4))
			{
				var hitbox = area2D as Hitbox;
				//Typecast the onwer of the hitbox to ITakeDamage
				ITakeDamage ownerTakeDamage = (ITakeDamage)Owner;
				//Call the TakeDamage method and pass in the damage and AttackFromVector values from the hitbox
				ownerTakeDamage.TakeDamage(hitbox.Damage, hitbox.AttackFromVector);
			}
			
		}
		
	}
}
