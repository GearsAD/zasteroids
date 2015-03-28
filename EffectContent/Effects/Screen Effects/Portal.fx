//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

//The parameters for our light source.
float2 portalEpicentre;				// The Location of the light source
float portalRadius = 50.0f;

float4 LightingShader(float2 texCoord: TEXCOORD0) : COLOR
{

	//Get the current pixel colour before it is processed...
	float4 pixelColour		= tex2D(ScreenS, texCoord);
	//float4 originalPixel	= pixelColour;
	
	float dist = length(texCoord - portalEpicentre);
	
	if((portalRadius < dist) && (dist == 0))
	{
		float2 innerCoord	= texCoord;
		innerCoord[0]		-= 1;
		innerCoord[1]		-= 1;
		float4 innerColour	= tex2D(ScreenS, innerCoord);
	}
	
	// 
	return pixelColour;
}

//This is how we define what pixel shader function to use.
technique
{
    pass P0
    {
        PixelShader = compile ps_2_0 LightingShader();
    }
}
