using Godot;
using System;

public partial class SpikedFly : CharacterBody2D, ITakeDamage
{
	[Export]
	public float Health = 10.0f;
	[Export]
	public float SpikedFlySpeedMax = 200;
	[Export]
	public float SpikedFlySpeedMin = 900;
	[Export]
	public float SpikedFlySpeed = 0;
	
	private Timer _takeDamageTimer;
	private Vector2 _velocity = new Vector2(-100,0);
	private Vector2 _knockBack = Vector2.Zero;
	private Label _healthPoints;
	private AnimatedSprite2D _cloudDeathEffect;
	private bool _dead = false;
	private int _lastCloudDeathFrame;
	private int _scoreAmount = 10;

	private const int DIRECTION_RANGE = 200;
	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

	[Signal]
	public delegate void SpikedFlyDeadEventHandler(int scoreAmount);
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
		SpikedFlySpeed = _randomNumberGenerator.RandfRange(SpikedFlySpeedMin, SpikedFlySpeedMax);
		GD.Print(SpikedFlySpeed);
		_takeDamageTimer = GetNode<Timer>("TakeDamageTimer");
		_healthPoints = GetNode<Label>("HealthPoints/HealthLabel");
		_cloudDeathEffect = GetNode<AnimatedSprite2D>("CloudDeathEffect");
		_healthPoints.Text = "HP: " + Health;
		//get all the frames -1 (start in 0
		_lastCloudDeathFrame = _cloudDeathEffect.SpriteFrames.GetFrameCount("Dissolve") -1;
	}

	public void TakeDamage(float damage, Vector2? attackFromVector)
    {
	    // Decrease the health of the spiked fly
        Health -= damage;     
        _healthPoints.Text = "HP: " + Health;
        
        // If health is below or equal to zero & it's not dead
        if(Health <= 0 && !_dead)
        {
            _dead = true;
            EmitSignal("SpikedFlyDead", _scoreAmount);
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = false;
            _cloudDeathEffect.Visible = true;
            _cloudDeathEffect.Play("Dissolve");
        }
        else
        {
            // If the attack from vector isn't null
            if(attackFromVector != null)
            {
                // Knockback 
                // Get the angle from the attack
                var angle = Position.AngleTo((Vector2)attackFromVector);       
                // Calculate the new direction
                var direction = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));   
                // Set the new direction for the sprite
                Velocity = direction * DIRECTION_RANGE;                                    
            }
            // Set the color of the sprite to magenta
            Modulate = new Color(1,0,1,1); 
            
            // Start the take damage timer
            _takeDamageTimer.Start();            
        }
    }
    private void OnTakeDamageTimerTimeout()
    {
	    // Restore color to normal
        Modulate = new Color(1,1,1,1);    
    }    
    public override void _Process(double delta)
     {
        // If the sprite is dead, and the final frame in the death cloud animation has played
        if(_dead && _cloudDeathEffect.Frame == _lastCloudDeathFrame)
        {
	        // Free the sprite from memory
            QueueFree();   
            return;
        }
        // If the y-velocity isn't zero
        if(Velocity.Y != 0)
        {
            // Lerp the position back to zero
            var yVel = Mathf.Lerp(Velocity.Y, 0, 0.01f);
            Velocity = new Vector2(Velocity.X, yVel);
        }
        // If the speed of the sprite isn't at it's normal value
        if(Velocity.X > -SpikedFlySpeed)
        {
            // Move the speed back towards it's normal value
            var xVel  = SpikedFlySpeed * delta;
            Velocity -= new Vector2((float)xVel, Velocity.Y);
        }
        // If the sprite still has health
        if(Health > 0)
        {
            // Run the built-in move and slide method to move the sprite with physics
            MoveAndSlide();
        }
     }
}
