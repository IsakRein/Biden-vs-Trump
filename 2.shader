shader "Custom/BLUR2" { 
	fixed4 frag(v2f i) : SV_TARGET{
	    //init color variable
	    float4 col = 0;
	    for(float index=0;index<10;index++){
	        //add color at position to color
	        col += tex2D(_MainTex, i.uv);
	    }
	    //divide the sum of values by the amount of samples
	    col = col / 10;
	    return col;
	}
}