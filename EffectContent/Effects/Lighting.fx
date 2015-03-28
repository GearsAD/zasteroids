//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

//The parameters for our light source.
float3 lightAmbient;   // The ambient light
float2 lightSourceLocation;   // The Location of the light source
float3 lightColour; //The colour of the light 

float4 LightingShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float3 pixelLightComponents = {lightColour[0], lightColour[1], lightColour[2]}; 
	float2 texCoordPixels = texCoord;
	texCoordPixels[0] *= 800; //Convert the texture coordinates (from 0 to 1) to screen coordinates (800, 600).
	texCoordPixels[1] *= 600;
	
	//Calculate the distance between the light source and the pixel
	float dist = length(texCoordPixels - lightSourceLocation) / 8;
	
	//Calculate the effect of the light source on the pixel
	pixelLightComponents[0] /= dist;
	pixelLightComponents[1] /= dist;
	pixelLightComponents[2] /= dist;
	
	//Now add in the ambient light because it's independent of the distance
	pixelLightComponents[0] += lightAmbient[0];
	pixelLightComponents[1] += lightAmbient[1];
	pixelLightComponents[2] += lightAmbient[2];

	//Make sure that the light components (R,G,B) can't be greater than 1.5.
	if (pixelLightComponents[0] > 1.5) pixelLightComponents[0] = 1.5;
	if (pixelLightComponents[1] > 1.5) pixelLightComponents[1] = 1.5;
	if (pixelLightComponents[2] > 1.5) pixelLightComponents[2] = 1.5;

	//Get the current pixel colour before it is processed...
	float4 pixelColour = tex2D(ScreenS, texCoord);
	
	//Multiply each of the light's R,G,B, A values with the current pixel colour
	pixelColour[0] *= pixelLightComponents[0];
	pixelColour[1] *= pixelLightComponents[1];
	pixelColour[2] *= pixelLightComponents[2];
	pixelColour[3] = 1; //Make the pixel fully opaque.

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
