//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

//The parameters for our shock wave.
float2 shockCentre = {400, 300};   // The location of the shockwave in camera pixels
float shockRadius = 100;   // The radius of the shockwave in pixels
float shockWidth = 50; //The width of the shockwave in pixels
float shockAmplitude = 10; //The amplitude of the shockwave in pixels
float2 screenSize = {1280,768}; // The size of the screen.

float4 ShockWaveShader(float2 texCoord: TEXCOORD0) : COLOR
{
	//The pixel we're finally going to pick...
	float2 curPixelLoc = texCoord;
	curPixelLoc[0] *= screenSize[0];
	curPixelLoc[1] *= screenSize[1];
	
	float2 texCoordPixels = texCoord;
	texCoordPixels[0] *= screenSize[0]; //Convert the texture coordinates (from 0 to 1) to screen coordinates (800, 600).
	texCoordPixels[1] *= screenSize[1];
	float2 rVec = texCoordPixels - shockCentre;
	
	//Calculate the distance between the current pixel and the shock wave
	float dist = length(rVec);
	
	//Calculate the normalized pointing vector
	float2 rVecNorm = rVec / dist;
	
	//Only calculate it if the current pixel is inside the shock wave...
	if(dist >= shockRadius - shockWidth / 2.0 && dist <= shockRadius + shockWidth / 4.0)
	{
		float distFromPeak = shockRadius - dist;
		float curAmplitude = 0;
		if(distFromPeak <= 0) //We're behind the peak
		{
			curAmplitude = (shockAmplitude / 2.0) * cos(2 * 3.14159 * distFromPeak / shockWidth ) + (shockAmplitude / 2.0);
		} else //We're in front of the peak
		{
			curAmplitude = (shockAmplitude / 2.0) * cos(2 * 3.14159 * distFromPeak / (shockWidth/2.0)) + (shockAmplitude / 2.0);
		}
		
		float2 distortionVec = rVecNorm * curAmplitude;
		curPixelLoc += distortionVec;
	}
	
	//To get it back to UV space...
	curPixelLoc[0] /= screenSize[0]; 
	curPixelLoc[1] /= screenSize[1];
	return tex2D(ScreenS, curPixelLoc);	
}

//This is how we define what pixel shader function to use.
technique
{
    pass P0
    {
        PixelShader = compile ps_2_0 ShockWaveShader();
    }
}
