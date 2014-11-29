using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MineLib.Network.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MineLib.Network.Modern
{
    public static class Asn1
    {
        public static byte[] CreateSecretKey()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            var privateKey = new byte[16];
            rng.GetBytes(privateKey);

            return privateKey;

            //var generator = new CipherKeyGenerator();
            //generator.Init(new KeyGenerationParameters(new SecureRandom(), 128));
            //
            //return generator.GenerateKey();
        }

        public static string GetServerIDHash(byte[] publicKey, byte[] secretKey, string serverID)
        {
            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.ASCII.GetBytes(serverID));
            hashlist.AddRange(secretKey);
            hashlist.AddRange(publicKey);

            return JavaHelper.JavaHexDigest(hashlist.ToArray());
        }

        public static RSAParameters GetRsaParameters(byte[] publicKey)
        {
            var kp = PublicKeyFactory.CreateKey(publicKey);
            var rsaKeyParameters = kp as RsaKeyParameters;

            return DotNetUtilities.ToRSAParameters(rsaKeyParameters);
        }

        public static byte[] EncryptData(RSAParameters rsaParameters, byte[] data)
        {
            var cryptoService = new RSACryptoServiceProvider();
            cryptoService.ImportParameters(rsaParameters);
            return cryptoService.Encrypt(data, false);
        }
    }
}
