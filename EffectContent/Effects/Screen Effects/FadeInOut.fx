
// Gray out all colours as a screen effect

sampler textureSample : register(s0);

bool increaseRed	= false;
bool increaseGreen	= false;
bool increaseBlue	= false;

float redIncreaseFactor		= -0.02;
float greenIncreaseFactor	= -0.02;
float blueIncreaseFactor	= -0.02;

float4 FadeInOutShader(float2 Tex: TEXCOORD0) : COLOR
{
	float4 pixelColour = tex2D(textureSample, Tex);	
	
	// Keep our alphachannel at 1.
	//pixelColour.a = 1.0f;
	
	if(increaseRed)
	{
		if(pixelColour.r == 0.0f)
		{
			pixelColour.r = 0.2;
		}
	}
	pixelColour.r += pixelColour.r * redIncreaseFactor;
	pixelColour.r = saturate(pixelColour.r);

	if(increaseGreen)
	{
		if(pixelColour.g == 0.0f)
		{
			pixelColour.g = 0.2;
		}
	}
	pixelColour.g += pixelColour.g * greenIncreaseFactor;
	pixelColour.g = saturate(pixelColour.g);

	if(increaseBlue)
	{
		if(pixelColour.b == 0.0f)
		{
			pixelColour.b = 0.2;
		}
	}
	pixelColour.b += pixelColour.b * blueIncreaseFactor;
	pixelColour.b = saturate(pixelColour.b);
	
    return pixelColour;
}

technique PostProcess
{
	pass P0
	{
		// A post process shader only needs a pixel shader.
		PixelShader = compile ps_2_0 FadeInOutShader();
	}
}