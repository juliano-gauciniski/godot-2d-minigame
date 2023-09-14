using Godot;
using System;

public partial class LayersAndMasks : Node
{
	//Get Collision Layer by Name
	public uint GetCollisionLayerByName(string layerName)
	{
	    //Loop through all the 32 layers
	    for (uint i=1 ; i<=32 ; i++)
	    {
	        //Get the layer name
	        var layer = ProjectSettings.GetSetting("layer_names/2d_physics/layer_" + i).ToString();
	        
	        //If the layer name matches the layer name we are looking for
	        if (layer.Equals(layerName))
	        {
	            //Return the index of the layer
	            return i;
	        }
	    }
	    GD.Print("Could not find the " + layerName + " collisiom layer");
	    GD.Print("Make sure to set the collision layer name: " + layerName + " in Project Settings");

	    return 0;
	}
}
