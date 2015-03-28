
// Gray out all colours as a screen effect

sampler textureSample : register(s0);

float4 GrayScaleShader(float2 Tex: TEXCOORD0) : COLOR
{
	float4 pixelColour = tex2D(textureSample, Tex);	
	
	//pixelColour.rgb = (pixelColour.r + pixelColour.g + pixelColour.b)/3;
	pixelColour.rgb = dot(pixelColour.rgb, float3(0.3, 0.59, 0.11));

	// Keep our alphachannel at 1.
	pixelColour.a = 1.0f;
	
	//pixelColour.r = 1.0f;
	//pixelColour.g = 1.0f;
	//pixelColour.b = 1.0f;
		
    return pixelColour;
}

technique PostProcess
{
	pass P0
	{
		// A post process shader only needs a pixel shader.
		PixelShader = compile ps_2_0 GrayScaleShader();
	}
}