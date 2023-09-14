using Godot;
using System;

public partial class Main : Node2D
{
	private PackedScene _spikedFly;
	private Timer _spawnTimer;

	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

	private int _score = 0;
	private Label _scoreLabel;
	
	//paremeter for spawning the spiked fly
	const int SPIKE_FLY_X_POS_RANGE = 2000;
	const int SPIKE_FLY_Y_POS_RANGE_MIN = 100;
	const int SPIKE_FLY_Y_POS_RANGE_MAX = 1000;
	
	const int SPAWN_TIMER_DELAY_MAX = 4;
	
	[Signal]
	public delegate void GameOverEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Hide the mouse when the game starts
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Hidden);
		
		//Connect the custom GameOver signal to the OnGameOver method
		Callable callable = new Callable(this, MethodName.OnGameOver);
		Connect("GameOver", callable);

		_scoreLabel = GetNode<Label>("Score");
		
		//Initialize the random number generator
		_randomNumberGenerator.Randomize();
		
		//Get the _spawnTimer node
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		
		//Load the _spikedFly scene
		_spikedFly = ResourceLoader.Load<PackedScene>("res://Assets/Scenes/Enemies/SpikedFly.tscn");
		
		//Spawan a spiked fly
		SpawnSpikeFly();
	}

	//Method to spawn a spiked fly
	private void SpawnSpikeFly()
	{
		var yPos = _randomNumberGenerator.RandfRange(SPIKE_FLY_Y_POS_RANGE_MIN, SPIKE_FLY_Y_POS_RANGE_MAX);
		
		//Create a new instance of the _spikedFly scene
		var spikedFly = _spikedFly.Instantiate() as SpikedFly;
		spikedFly.Position = new Vector2(SPIKE_FLY_X_POS_RANGE, yPos);
		//Add the spiked fly to the scene
		AddChild(spikedFly);
		
		//Connect the custom SpikedFlyDead signal to the OnSpikedFlyDead method
		Callable callable = new Callable(this, MethodName.OnSpikedFlyDead);
		spikedFly.Connect("SpikedFlyDead", callable);
	}

	private void OnSpawnTimerTimeout()
	{
		SpawnSpikeFly();
		_spawnTimer.WaitTime = _randomNumberGenerator.RandfRange(1, SPAWN_TIMER_DELAY_MAX);
	}
	
	private void OnSpikedFlyDead(int score)
	{
		GD.Print("Spiked fly dead");
		_score += score;
		_scoreLabel.Text = "Score: " + _score;
	}

	private void OnGameOver()
	{
		//Call Game Over Scene
		GetTree().ChangeSceneToFile("res://Assets/Scenes/GameOver.tscn");
	}
}
