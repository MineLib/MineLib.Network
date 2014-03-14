using System.Drawing;

namespace MineLib.Network.Data
{
    public struct ServerVersion
    {
        public string Name;
        public int Protocol;
    }

    public struct Players
    {
        public int Max;
        public int Online;
    }

    public struct Sample
    {
    }

    public struct ServerInfo
    {
        public string Description;

        public Image Favicon;
        public Players Players;
        public Sample Sample;
        public ServerVersion Version;
    }
}