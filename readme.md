# FNAEngine

The goal of this project is  to make it easier to create new games with FNA-XNA. The FNA framework is really easy to use but with each new games or test project, i needed to recreate a little framework.

I'm a programmer for over 25 years, i did a little of Unity and i fell in love with FNA-XNA. It's user easy to use, to customize and super quick to develop and try new things.

I used .Net 4.8 because i still have issues with .Net 6.0/7.0. The Edit and continue si not as stable in Visual studio and it's the framework that i'm the most confortable with.

WARNING: This project is meant to learn XNA quand create little games for yourself.


# Working features

Some nice feature are already working:

- GameHost / GameObject structure
- TextureRender: a GameObject to draw texture on screen
- TextRenderer: a GameObject to draw text on screen
- Collider: Basic rectangle collision detection system
- Input: Basic input system (that i took from Michael Hicks: https://www.youtube.com/playlist?list=PL3wErD7ZIp_DtsTKoouVCxu81UQkI9VZL)
- ContentManager: Custom ContentManager that hot reloads the modified texture at runtime


# Exemple

Check my Pong test game that i created with this engine: https://github.com/Hilderin/Pong


# Dependencies

This projet uses:
- FNA-XNA-Wrapper (a nuget that i created to bundle FNA in a nuget package. Weirdly, nobody seems to use created one for that easy to use before or i just did not find it?!?)
- Velentr.Font.FNA: For drawing text


# Work in progress

I just begun working with FNA-XNA, there's so must that can be added and upgraded.

My plan is to upgrade my Pong game with different goodies and upgrade the engine at the same time.






