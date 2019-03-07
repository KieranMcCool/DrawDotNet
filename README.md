# Introduction

DrawDotNet is a work in progress multi-platform graphics/drawing library based on the [SDL-CS](https://github.com/flibitijibibo/SDL2-CS) bindings for [SDL.](https://www.libsdl.org/)

Given that SDL is a fairly low level, written in C, it is somewhat unintuitive to use with an OO Language like C# so I've tried my best to provide a nice framework that you can use in an Object Oriented way.

# What Works

* There's an entity based render loop, whereby you add drawable entities to the window and it will run their update and draw methods asynchronously. 
* You can draw rectangles.
