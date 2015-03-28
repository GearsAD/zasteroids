uniform extern texture ScreenTexture;  

sampler2D input : register(s0); 
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float darknessCoefficient = 0.00;
float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{

	float4 pixelColour = tex2D(ScreenS, texCoord);
	
	// Almost Pitch Black Effect
	float colourFactor = 0.01;

    pixelColour = tex2D( input , texCoord);
	//pixelColour.r -= pixelColour.r * darknessCoefficient;
	//pixelColour.g -= pixelColour.g * darknessCoefficient;
	//pixelColour.b -= pixelColour.b * darknessCoefficient;
	
	pixelColour.r -= ( darknessCoefficient ) * tex2D( input , texCoord + (colourFactor/100)).r;
	pixelColour.g -= ( darknessCoefficient ) * tex2D( input , texCoord + (colourFactor/200)).g;
	pixelColour.b -= ( darknessCoefficient ) * tex2D( input , texCoord + (colourFactor/300)).b;	
	
	return pixelColour;
	
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
