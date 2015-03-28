uniform extern texture ScreenTexture;  

//sampler2D input : register(s0); 
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{

	float4 pixelColour = tex2D(ScreenS, texCoord);

	pixelColour.r = 0.1495*pixelColour.r + 0.2935*pixelColour.g + 0.057*pixelColour.b;// + 0.5;
	pixelColour.g = 0.1495*pixelColour.r + 0.2935*pixelColour.g + 0.057*pixelColour.b;// + 0.25;
	pixelColour.b = 0.1495*pixelColour.r + 0.2935*pixelColour.g + 0.057*pixelColour.b;

	//pixelColour.r	= 0.1 * pixelColour.r;

	return pixelColour;

}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
