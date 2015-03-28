uniform extern texture ScreenTexture;  

sampler2D input : register(s0); 
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float lightMagnificationFactor = 0.00;
float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{

	float4 pixelColour = tex2D(ScreenS, texCoord);
	
	// Night Vision
	float colourFactor = 0.01;
	
    pixelColour = tex2D( input , texCoord);
	pixelColour.r -= (lightMagnificationFactor )	* tex2D( input , texCoord + (colourFactor/100)).r;
	pixelColour.g += (lightMagnificationFactor )	* tex2D( input , texCoord + (colourFactor/200)).g;
	pixelColour.b -= (lightMagnificationFactor )	* tex2D( input , texCoord + (colourFactor/300)).b;	
	
	//pixelColour.r += pixelColour.r * (0.50);
	//pixelColour.g += pixelColour.g * (0.50);
	//pixelColour.b += pixelColour.b * (0.50);

	return pixelColour;

}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
