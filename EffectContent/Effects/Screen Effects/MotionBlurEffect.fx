//This is the screen texture - the screen image (which we rendered earlier) is passed to this texture by the SpriteBatch.
uniform extern texture ScreenTexture;    

extern texture PriorScreenTexture;

//This sampler allows the pixel shader to sample the screen texture. We can't do that directly and must do it via a sampler.
sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};

sampler PriorScreenS: register(s1) = sampler_state
{
    Texture = <PriorScreenTexture>;    
};


float blendFraction = 0.75;


float4 MotionBlur(float2 texCoord: TEXCOORD0) : COLOR
{
	float4 pix = {1,1,0,1};
	pix = saturate(tex2D(ScreenS, texCoord) + tex2D(PriorScreenS, texCoord));
	//pix = blendFraction * tex2D(ScreenS, texCoord) + (1-blendFraction) * tex2D(PriorScreenS, texCoord);

    return pix;
}

technique
{
    pass P0
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 MotionBlur();
    }
}
