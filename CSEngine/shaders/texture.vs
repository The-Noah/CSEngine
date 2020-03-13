#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textureCoords;

out vec2 texCoords;

uniform mat4 projViewMatrix;
uniform mat4 modelMatrix;

void main(void){
    //gl_Position = vec4(position, 1.0);
		gl_Position = projViewMatrix * modelMatrix * vec4(position, 1.0);
    texCoords = textureCoords;
}