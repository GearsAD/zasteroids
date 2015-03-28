
// Global variables
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);

// Noise
float4 ParticleFlare(float2 Tex: TEXCOORD0) : COLOR
{	
	// Use our new texture coordinate to look-up a pixel in ColorMapSampler.
	float4 Color=tex2D(ColorMapSampler, Tex);

	// Keep our alphachannel at 1.
	Color.r = Color.r * Color.r;
	Color.g = Color.g * Color.g;
	Color.b = Color.b * Color.b *2;

    return Color;
}

technique FlareParticles
{
	pass P0
	{
		// A post process shader only needs a pixel shader.
		PixelShader = compile ps_2_0 ParticleFlare();
	}
}
