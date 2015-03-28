//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

//The parameters for our light source.
float Timer = 0;
float Intensity = 2;

//Nice effects
//http://www.truevision3d.com/forums/announcements/hlsl_post_process_shaders_for_newbies-t18745.0.html
//http://developer.download.nvidia.com/shaderlibrary/webpages/hlsl_shaders.html
//This one is straight out of the first link!

float4 VideoNoiseShader(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 c;
	c = tex2D( ScreenS, texCoord);
	c.r = c.r+(sin(texCoord.y*100*Timer)*Intensity);
	c.g = c.g+(cos(texCoord.y*200*Timer)*Intensity);
	c.b = c.b+(sin(texCoord.y*300*Timer)*Intensity);
	//c.rgb = (c.r+c.g+c.b)/1.0f;
	return c;
}

//This is how we define what pixel shader function to use.
technique
{
    pass P0
    {
        PixelShader = compile ps_2_0 VideoNoiseShader();
    }
}
