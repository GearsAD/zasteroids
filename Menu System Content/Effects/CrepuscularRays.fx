//This is the final texture for the sphere.
uniform extern texture CurrentTexture;    

#define NUM_SAMPLES 40

float2 halfPixel;
float2 LightScreenPos;

float Density = 1.15f;
float Decay = .85f;
float Weight = 4.0f;
float Exposure = .35f;

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler CurrentImage = sampler_state
{
    Texture = <CurrentTexture>;    
};

extern texture BlackoutTexture;
//This sampler samples the blackout texture.
sampler BlackoutImage: register(s2) = sampler_state
{
    Texture = <BlackoutTexture>;  
	AddressU = CLAMP;
	AddressV = CLAMP;  
};

float2 Viewport;

void SpriteVertexShader(inout float4 color    : COLOR0,
                       inout float2 texCoord : TEXCOORD0,
                       inout float4 position : POSITION0)

{
   // Half pixel offset for correct texel centering.
   position.xy -= 0.5;
   // Viewport adjustment.
   position.xy = position.xy / Viewport;
   position.xy *= float2(2, -2);
   position.xy -= float2(1, -1);
}


float4 LightRaysPS(float2 texCoord : TEXCOORD0) : COLOR
{
	float2 TexCoord = texCoord - halfPixel;

    float2 DeltaTexCoord = (TexCoord - LightScreenPos);
    DeltaTexCoord *= (1.0f / NUM_SAMPLES * Density);

    //DeltaTexCoord = DeltaTexCoord * clamp(ScreenPosition.w * ScreenPosition.z,0,.5f);

    float3 col = tex2D(BlackoutImage,TexCoord);
    float IlluminationDecay = 0.25;
    float3 Sample;
    
    for( int i = 0; i < NUM_SAMPLES; ++i )
    {
        TexCoord -= DeltaTexCoord;
        Sample = tex2D(BlackoutImage, TexCoord);
        Sample *= IlluminationDecay * Weight;
        col += Sample;
        IlluminationDecay *= Decay;            
    }
	return float4(col * Exposure,1);// * (ScreenPosition.w * .0025);
}

technique
{
    pass P0
    {
		VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 LightRaysPS();
    }
}
