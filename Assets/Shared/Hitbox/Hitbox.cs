using Godot;
using System;

public partial class Hitbox : Area2D
{
	[Export]
	// The damage the hitbox will deliver
	public float Damage = 1.0f;      
	// The vector the attack came from
	public Vector2? AttackFromVector = null;    
	public override void _Ready()
	{
		// Get the LayersAndMasks singleton
		//var layersAndMasks = GetNode<LayersAndMasks>("/root/LayersAndMasks");
		// Get the collision layer for the hitbox by name
		//CollisionLayer = layersAndMasks.GetCollisionLayerByName("Hitbox");  
		// Set collision mask to 0 = no mask
		//CollisionMask = 0;                                                     
	}
	public void SetAttackFromVector(Vector2 attackVector)
	{
		AttackFromVector = attackVector;
	}
}
