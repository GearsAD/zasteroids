//This is the final texture for the sphere.
uniform extern texture CurrentTexture;    

#define SAMPLE_COUNT 15

float2 SampleOffsets[SAMPLE_COUNT];
float SampleWeights[SAMPLE_COUNT];

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler CurrentImage = sampler_state
{
    Texture = <CurrentTexture>;    
};

extern texture RaysTexture;
//This sampler provides the rays texture to the .
sampler RaysImage: register(s2) = sampler_state
{
    Texture = <RaysTexture>;  
};

float2 Viewport;

float4 CombinerPS(float2 texCoord : TEXCOORD0) : COLOR
{
    float4 c = 0;
    
    // Combine a number of weighted image filter taps.
    for (int i = 0; i < SAMPLE_COUNT; i++)
    {
        c += tex2D(RaysImage, texCoord + SampleOffsets[i]) * SampleWeights[i];
    }
	return c + tex2D(CurrentImage, texCoord);
}

technique
{
    pass P0
    {
//		VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_2_0 CombinerPS();
    }
}
