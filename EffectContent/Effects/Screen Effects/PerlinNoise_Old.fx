

uniform extern texture ScreenTexture;  

sampler2D input : register(s0); 
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

//Global Variables
float Timer : TIME;

//noise effect intensity value (0 = no effect, 1 = full effect)
float fNintensity = 0.5;
//scanlines effect intensity value (0 = no effect, 1 = full effect)
float fSintensity = 0.1;
//scanlines effect count value (0 = no effect, 4096 = full effect)
float fScount = 2048;

float lightMagnificationFactor = 0.00;
float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{

	float4 pixelColour = tex2D(ScreenS, texCoord);
	//float colourFactor = 0.01;
	vector perlinNoiseSeed = 5;
    //pixelColour = tex2D( input , texCoord);
	pixelColour[0] = noise(perlinNoiseSeed);
	pixelColour[1] = noise(perlinNoiseSeed);
	pixelColour[2] = noise(perlinNoiseSeed);
	
	//float3 cResult = cTextureScreen.rgb + cTextureScreen.rgb * saturate(0.1f + dx.xxx * 100);
	//return pixelColour;
	
	// sample the source
	//float4 cTextureScreen = tex2D( ScreenS, texCoord);

	// make some noise
	//float x = texCoord.x * texCoord.y * Timer *  1000;
	//x = fmod(x, 13) * fmod(x, 123);	
	//float dx = fmod(x, 0.01);
	
	// add noise
	//float3 cResult = cTextureScreen.rgb + cTextureScreen.rgb * saturate(0.1f + dx.xxx * 100);
	
	//vector seed = 5;
	//cTextureScreen[0] = noise(seed);
	
	
	// get us a sine and cosine
	//float2 sc; sincos(texCoord.y * fScount, sc.x, sc.y);

	// add scanlines
	//cResult += cTextureScreen.rgb * float3(sc.x, sc.y, sc.x) * fSintensity;
	
	// interpolate between source and result by intensity
	//cResult = lerp(cTextureScreen, cResult, saturate(fNintensity));

	// convert to grayscale if desired
	//#ifdef MONOCHROME
	//	cResult.rgb = dot(cResult.rgb, float3(0.3, 0.59, 0.11));
	//#endif

	// return with source alpha
	//return float4(cResult, cTextureScreen.a);
	
	return pixelColour;

}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

/*****************************************************************************************************************************************************

// Screen Space Static Postprocessor
//
// Produces an analogue noise overlay similar to a film grain / TV static
//
// Original implementation and noise algorithm
// Pat 'Hawthorne' Shearon
//
// Optimized scanlines + noise version with intensity scaling
// Georg 'Leviathan' Steinröhder
//
// This version is provided under a Creative Commons Attribution 3.0 License
// http://creativecommons.org/licenses/by/3.0/ 
// 

//Global Variables
float Timer : TIME;

//noise effect intensity value (0 = no effect, 1 = full effect)
float fNintensity = 0.5;
//scanlines effect intensity value (0 = no effect, 1 = full effect)
float fSintensity = 0.1;
//scanlines effect count value (0 = no effect, 4096 = full effect)
float fScount = 2048;

#define RGB
//#define MONOCHROME

texture texScreen;
sampler2D sampScreen = sampler_state
{
    Texture = <texScreen>;
    MinFilter = Point;
    MagFilter = Point;
    AddressU = Wrap;
    AddressV = Wrap;
};  
 
// Input and Output Semantics 
struct VS_INPUT 
{ 
   float4 position : POSITION; 
   float2 texCoord : TEXCOORD; 
};

struct VS_OUTPUT 
{ 
   float4 position : POSITION; 
   float2 texCoord : TEXCOORD;    
}; 

#define PS_INPUT VS_OUTPUT 

//Vertex Shader Output
VS_OUTPUT vs_main(const VS_INPUT IN) 
{ 
   VS_OUTPUT OUT; 
   OUT.position = IN.position; 
   OUT.texCoord = IN.texCoord; 
   return OUT; 
} 

// Pixel Shader Output
float4 ps_main( float2 Tex : TEXCOORD0  ) : COLOR0 
{
	// sample the source
	float4 cTextureScreen = tex2D( sampScreen, Tex.xy);

	// make some noise
	float x = Tex.x * Tex.y * Timer *  1000;
	x = fmod(x, 13) * fmod(x, 123);	
	float dx = fmod(x, 0.01);
	
	// add noise
	float3 cResult = cTextureScreen.rgb + cTextureScreen.rgb * saturate(0.1f + dx.xxx * 100);

	// get us a sine and cosine
	//float2 sc; sincos(Tex.y * fScount, sc.x, sc.y);

	// add scanlines
	//cResult += cTextureScreen.rgb * float3(sc.x, sc.y, sc.x) * fSintensity;
	
	// interpolate between source and result by intensity
	cResult = lerp(cTextureScreen, cResult, saturate(fNintensity));

	// convert to grayscale if desired
	//#ifdef MONOCHROME
	//	cResult.rgb = dot(cResult.rgb, float3(0.3, 0.59, 0.11));
	//#endif

	// return with source alpha
	return float4(cResult, cTextureScreen.a);
} 

//Technique Calls
technique PostProcess 
{ 
   pass Pass_0 
   { 
      //VertexShader = compile vs_1_1 vs_main(); 
      PixelShader = compile ps_2_0 ps_main(); 
   } 
} 
*/

/************************************************************************************************************

uniform extern texture ScreenTexture;  

sampler2D input : register(s0); 
sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{

	//float4 pixelColour = tex2D(ScreenS, texCoord);
	
	float4 pixelColour = tex2D( input , texCoord);
	
	vector perlinNoiseSeed = 1.01f;
	
	//float noiseFactor = noise(perlinNoiseSeed);
	//pixelColour.rgb = noiseFactor;
	//pixelColour[0] /= noiseFactor;
	//pixelColour[1] /= noiseFactor;
	//pixelColour[2] /= noiseFactor;
	
	float noiseFactor = noise(perlinNoiseSeed);
	pixelColour[0] = noiseFactor;
	noiseFactor = noise(noiseFactor);
	pixelColour[1] = noiseFactor;
	noiseFactor = noise(noiseFactor);
	pixelColour[2] = noiseFactor;
	
	return pixelColour;

}

technique Technique1
{
    pass Pass1
    
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

/***************************************************************************

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

*/
