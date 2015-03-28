//This is the final texture for the sphere.
uniform extern texture CurrentTexture;    

//The last sphere texture.
extern texture LastImageTexture;

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler CurrentImage = sampler_state
{
    Texture = <CurrentTexture>;    
};

//This sampler samples the current video image.
//Have to use the second texture channel - spritebatch steals the first! This.... was.... a.... pain.... to.... find.... sigh....
sampler LastImage: register(s1) = sampler_state
{
    Texture = <LastImageTexture>;    
};

float previousImageDecayRateS; //The rate at which the earlier images decay.
float elapsedTimeS; //The time that has passed. 

//The world matrix - should not be used.
float4x4 MatrixTransform;

//Render the current image onto the video sphere texture.
float4 MotionBlurPS(float2 texCoord: TEXCOORD0) : COLOR
{
	//Initialize a return value - set to the prior image by default.
	float4 lastpix = tex2D(LastImage, texCoord);
	float4 pix = tex2D(CurrentImage, texCoord);

	for(int i = 0; i < 3; i++) //Decay the red and green components...
		pix[i] = saturate(pix[i] * 0.3f + lastpix[i] * 0.7f); //Cheap as hell, for now, unless we need to make it fancier.

	pix[3] = 1.0f;
    return pix;
}

void DummyVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{ 
    position = position;//mul(position, MatrixTransform); 
} 

technique
{
    pass P0
    {
		//VertexShader = compile vs_3_0 DummyVertexShader();
        PixelShader = compile ps_2_0 MotionBlurPS();
    }
}
