MineLib.Network
===============

Library for handling network connection with Minecraft server.

My implementation of How-it-should-be.

All server and clients packets for 1.7.4 are supported.
Only logic here - automatically enable encryption. You don't need to manually handle with that.

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
