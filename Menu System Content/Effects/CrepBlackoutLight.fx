#define NUM_SAMPLES 20

//The light parameters and tweaks
float2 LightScreenPos;

//float2 halfPixel;

float Density = .5f;
float Decay = .90f;
float Weight = 2.0f;
float Exposure = .15f;

//This is the final texture for the sphere.
uniform extern texture CurrentTexture;    
//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler CurrentImage = sampler_state
{
    Texture = <CurrentTexture>;    
};

//The last sphere texture.
extern texture LenseFlareTexture;
//This sampler samples the lens flare.
sampler LenseFlareImage: register(s1) = sampler_state
{
    Texture = <LenseFlareTexture>;  
	AddressU = CLAMP;
	AddressV = CLAMP;  
};

extern texture BlackoutTexture;
//This sampler samples the blackout texture.
sampler BlackoutImage: register(s2) = sampler_state
{
    Texture = <BlackoutTexture>;  
	AddressU = CLAMP;
	AddressV = CLAMP;  
};

//Render the lighting underlay for the crepuscular lighting.
float4 LightingUnderlayPS(float2 texCoord: TEXCOORD0) : COLOR
{
	//Initialize a return value - set to the prior image by default.
	float4 pix = tex2D(CurrentImage, texCoord);
	float4 blackoutpix = tex2D(BlackoutImage, texCoord);
	if(blackoutpix[0] == 0 && blackoutpix[1] == 0 && blackoutpix[2] == 0) 
	{
		return blackoutpix;
	}else
	{	
		//UV lookup.
		if(LightScreenPos.x >= -0.25 && LightScreenPos.y >= -0.25 && LightScreenPos.x <= 1.25 && LightScreenPos.y <= 1.25)
		{
			float2 lookup = LightScreenPos - texCoord;
			lookup.x = lookup.x*2.0f + 0.5f;
			lookup.y = lookup.y*2.0f + 0.5f;
//			if(lookup.x > 0 && lookup.y > 0 && lookup.x < 1 && lookup.y < 1)
				return tex2D(LenseFlareImage, lookup);
//			else 
			{
				float4 black = {0,0,0,1};
				return black;
			}
		}
	}
	return pix;
}

/*float4 LightRaysPS(float2 texCoord : TEXCOORD0) : COLOR
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
	return float4(col * Exposure,1) + tex2D(CurrentImage, texCoord);// * (ScreenPosition.w * .0025);
}
*/
technique
{
    pass P0

    {
		//VertexShader = compile vs_3_0 DummyVertexShader();
        PixelShader = compile ps_2_0 LightingUnderlayPS();
    }
//	pass P1
//	{
//        PixelShader = compile ps_2_0 LightRaysPS();
//	}
}
