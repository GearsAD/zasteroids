//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

//An example parameter
float screenWidth = 1.0;
float screenHeight = 1.0;
float gridWidth = 25;
float gridHeight = 25;
float gridZHeightPixels = 15;
float timerMS = 0;
float4 gridColour = {0.9, 0.9, 0.9, 1};
float4 backgroundColour = {0.25, 0.25, 0.25, 1};
float4 radianceColour = {0.7, 0.7, 1, 1};

/// Calculates the distance from the current point to the nearest radiance point (the source grid)
float CalculateDistanceFromGridToPoint(float x, float y)
{
	float2 closestGridLoc = {0,0};
	//Four zones for the different pespectives
	if(x <= screenWidth /2.0) //We're on the left.
		closestGridLoc.x = ceil(x / gridWidth) * gridWidth;
	else
		closestGridLoc.x = floor(x / gridWidth) * gridWidth;
	if(y <= screenHeight /2.0) //We're on the top .
		closestGridLoc.y = ceil(y / gridHeight) * gridHeight;
	else
		closestGridLoc.y = floor(y / gridHeight) * gridHeight;

	//If x is greater than y  then use the y-axis grid position, otherwise use the x-axis grid position (as the actual value).
	//This is the closest point to the closest ray, not actual grid point.
	if(abs(x - closestGridLoc.x) > abs(y - closestGridLoc.y))
		closestGridLoc.y = y;
	else
		closestGridLoc.x = x;
	float2 vec = {x - closestGridLoc.x, y - closestGridLoc.y};
	return length(vec);
}

//Draws a surreal grid over the background - use for background of game.
float4 BackgroundGrid(float2 texCoord: TEXCOORD0) : COLOR
{
	//Get the pixel location of the point.
    int x = (texCoord.x * screenWidth);
	int y = (texCoord.y * screenHeight);
	float4 pix = {1,1,0,1};

	//If part of grid, draw it.
//	if(x % gridWidth == 0 || y % gridHeight == 0)
//	{
//		pix = gridColour;
//	}
//	else
	{
		//Calculate the radiance then.
		//Get a pointing vector
		float2 pointingVec = {(x - screenWidth / 2.0) / (screenWidth/2.0), (y - screenHeight /2.0) / (screenHeight/2.0)};
		//Normalise it
		//pointingVec /= length(pointingVec);
		
		//Get the ratio between the distance and the max distance
		float radiancePower = (1.0 - saturate(CalculateDistanceFromGridToPoint(x,y) / gridZHeightPixels)) * length(pointingVec);
		pix = radianceColour * radiancePower + backgroundColour;
	}

    return pix;
}

technique
{
    pass P0
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 BackgroundGrid();
    }
}
