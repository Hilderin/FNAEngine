# FNAEngine

The goal of this project is  to make it easier to create new games with FNA-XNA. The FNA framework is really easy to use but with each new games or test project, i needed to recreate a little framework.

I'm a programmer for over 25 years, i did a little of Unity and i fell in love with FNA-XNA. It's user easy to use, to customize and super quick to develop and try new things.

I used .Net 4.8 because i still have issues with .Net 6.0/7.0. The Edit and continue si not as stable in Visual studio and it's the framework that i'm the most confortable with.

WARNING: This project is meant to learn XNA quand create little games for yourself.


# Working features

Some nice feature are already working:

- GameHost / GameObject structure
- TextureRender: GameObject to draw texture on screen
- TextRenderer: GameObject to draw text on screen with different alignments
- Collider: Basic rectangle collision detection system
- RigidBody: Basic rigifbody to simulate physics in 2D (platform/side scrolling)
- Input: Basic input system (that i took from Michael Hicks: https://www.youtube.com/playlist?list=PL3wErD7ZIp_DtsTKoouVCxu81UQkI9VZL)
- ContentManager: Custom ContentManager that hot reloads the modified texture at runtime
- IMouseEventHandler: Implement Mouse Down, Up and Click on GameObject
- Aseprite format integration for textures and sprite animations. (checkout Asperite, it's very nice for sprites and Pixel Arts: https://github.com/aseprite/aseprite (it's free if you build it yourself).


# Exemple

Check my test games that i created with this engine: 
- Pong: https://github.com/Hilderin/Pong
- Platformer: https://github.com/Hilderin/Platformer



# Dependencies

This projet uses:
- FNA-XNA-Wrapper (a nuget that i created to bundle FNA in a nuget package. Weirdly, nobody seems to use created one for that easy to use before or i just did not find it?!?)
- Velentr.Font.FNA: For drawing text


# Work in progress

I just begun working with FNA-XNA, there's so must that can be added and upgraded.

My plan is to upgrade my Pong game with different goodies and upgrade the engine at the same time.






