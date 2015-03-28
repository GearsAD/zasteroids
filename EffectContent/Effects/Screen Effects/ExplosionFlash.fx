// This is the screen texture - the screen image (which we rendered earlier) is 
// passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

// Following VARS used as Dummy fields for Dummy Vertex Shader
float4x4 World;
float4x4 View;
float4x4 Projection;
// End of Dummy Vars

//The parameters for our light source.
float3 lightAmbient;					// The ambient light
float2 lightSourceLocation;				// The Location of the light source
float3 lightColour;						// The colour of the light 
float lightAttenuationFactor = 3.8f;	// The light/distance attenuation - we'll divide the pixel/light source distance by this. ; Default = 1.8f
bool allowOverExposure		 = false;	// Allow Pixel Magnitude to exceed max
bool useRadialDegradation	 = false;

float ScreenHeight;
float ScreenWidth;

float4 LightingShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float3 pixelLightComponents = {lightColour[0], lightColour[1], lightColour[2]}; 
	float2 texCoordPixels = texCoord;
	texCoordPixels[0] *= ScreenWidth; //Convert the texture coordinates (from 0 to 1) to screen coordinates (800, 600). // ScreenWidth
	texCoordPixels[1] *= ScreenHeight;  // ScreenHeight
	
	//Calculate the distance between the light source and the pixel
	float dist = length(texCoordPixels - lightSourceLocation);
	//Scale the distance - this is a tweaking factor
	dist /= lightAttenuationFactor;
	
	//Calculate the effect of the light source on the pixel
	pixelLightComponents[0] /= dist;
	pixelLightComponents[1] /= dist;
	pixelLightComponents[2] /= dist;
	
	//Now add in the ambient light because it's independent of the distance
	pixelLightComponents[0] += lightAmbient[0];
	pixelLightComponents[1] += lightAmbient[1];
	pixelLightComponents[2] += lightAmbient[2];

	//Make sure that the light components (R,G,B) can't be greater than 1.5.
	if(!allowOverExposure)
	{
		if (pixelLightComponents[0] > 1.5) 
			pixelLightComponents[0] = 1.5;
		if (pixelLightComponents[1] > 1.5) 
			pixelLightComponents[1] = 1.5;
		if (pixelLightComponents[2] > 1.5) 
			pixelLightComponents[2] = 1.5;
	}

	//Get the current pixel colour before it is processed...
	float4 pixelColour		= tex2D(ScreenS, texCoord);
	float4 originalPixel	= pixelColour;
	
	//Multiply each of the light's R,G,B, A values with the current pixel colour
	pixelColour[0] *= pixelLightComponents[0];
	pixelColour[1] *= pixelLightComponents[1];
	pixelColour[2] *= pixelLightComponents[2];
	pixelColour[3] = 1; //Make the pixel fully opaque.
	
	//pixelColour[0] = 0.0;
	//pixelColour[1] = 0.0;
	//pixelColour[2] = 0.0;

	// If the resultant pixel's light is lower than the normal light for the pixel
	if(pixelColour[0] < originalPixel[0])
	{
		pixelColour[0] = originalPixel[0];
	}
	if(pixelColour[1] < originalPixel[1])
	{
		pixelColour[1] = originalPixel[1];
	}
	if(pixelColour[2] < originalPixel[2])
	{
		pixelColour[2] = originalPixel[2];
	}
	// End of Light Level Check

	return pixelColour;
}

struct VertexShaderInput
{
    float4 Position : POSITION0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput DummyVertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // TODO: add your vertex shader code here.

    return output;
}

//This is how we define what pixel shader function to use.
technique
{
    pass P0
    {
		//VertexShader	= compile vs_2_0 DummyVertexShaderFunction();
        PixelShader		= compile ps_2_0 LightingShader();
    }
}

