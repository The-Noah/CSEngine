#version 330 core

in vec2 texCoords;

out vec4 out_Color;

uniform sampler2D textureSampler;

void main(void){
		out_Color = texture(textureSampler, texCoords);
		//out_Color = vec4(texCoords.x, texCoords.y, 0, 1);
}