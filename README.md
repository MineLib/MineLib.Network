[![Build Status](https://travis-ci.org/Aragas/MineLib.Network.svg)](https://travis-ci.org/Aragas/MineLib.Network) [![Build status](https://ci.appveyor.com/api/projects/status/26t4lc01vh3qxfle)](https://ci.appveyor.com/project/Aragas/minelib-network)
MineLib.Network
===============

Library for handling network connection with Minecraft server.

My implementation of How-it-should-be.

Supported Main(1.8) and Classic(Without Online mode yet) protocols.

Not all server and clients packets for 1.8 are fully supported yet. Few have some problems. Compression is fully supported.

You can use Events based packet handling (use RaisePacketHandledUnUsed() in NetworkHandler.Packets.cs) or handle dat stuff manually (use OnPacketHandled for that).

Supported:
* Yggdrasil
* Online && Pirate mode
* Nah, i think all basic stuff is supported


Documentation will come soon.
See https://github.com/Aragas/MineLib.ClientWrapper for "How use dat shiet"

Used repos:
* https://github.com/umby24/libMC.NET
* https://github.com/Conji/the-syhno-project
* https://github.com/pollyzoid/mclib
* https://github.com/pdelvo/Pdelvo.Minecraft
* https://github.com/SirCmpwn/Craft.Net
* https://github.com/GlowstoneMC/Glowstone
