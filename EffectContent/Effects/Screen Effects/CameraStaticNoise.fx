/*
 * Tutorial
 * XNA Shader programming
 * www.gamecamp.no
 * 
 * by: Petri T. Wilhelmsen
 * e-mail: petriw@gmail.com
 * 
 * Feel free to ask me a question, give feedback or correct mistakes!
 * This shader is mostly based on the shader "post edgeDetect" from nVidias Shader library:
 * http://developer.download.nvidia.com/shaderlibrary/webpages/shader_library.html
 */


// Global variables
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);

uniform extern texture NoiseTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler NoiseS = sampler_state
{
    Texture = <NoiseTexture>;    
};

// A timer to animate our shader
float fTimer;

// the amount of distortion
float fNoiseAmount;

// just a random starting number
int iSeed;

// Noise Quantity
float yVariation = 0.040f; // Default : 0.040

// Noise
float4 CameraStaticNoise(float2 Tex: TEXCOORD0) : COLOR
{
	// Distortion factor
	float NoiseX = iSeed * fTimer * sin(Tex.x * Tex.y+fTimer);
	NoiseX=fmod(NoiseX,8) * fmod(NoiseX,4);

	// Use our distortion factor to compute how much it will affect each
	// texture coordinate
	float DistortX = fmod(NoiseX,fNoiseAmount);
	float DistortY = fmod(NoiseX,fNoiseAmount*yVariation);  // 0.002 ; 0.040
	
	// Create our new texture coordinate based on our distortion factor
	float2 DistortTex = float2(DistortX, DistortY);
	
	// Use our new texture coordinate to look-up a pixel in ColorMapSampler.
	float4 Noise=(tex2D(NoiseS, Tex + DistortTex) - 0.5) * (fNoiseAmount/2.0);
	Noise.a = 1.0;
	float4 Color=tex2D(ColorMapSampler, Tex) + Noise;
	
	// Keep our alphachannel at 1.
	Color.a = 1.0f;

    return Color;
}

technique PostProcess
{
	pass P0
	{
		// A post process shader only needs a pixel shader.
		PixelShader = compile ps_2_0 CameraStaticNoise();
	}
}
