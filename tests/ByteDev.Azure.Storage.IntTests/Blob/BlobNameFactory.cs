using ByteDev.Crypto.Random;

namespace ByteDev.Azure.Storage.IntTests.Blob
{
    public static class BlobNameFactory
    {
        public static string Create()
        {
            return Create(string.Empty);
        }

        public static string Create(string prefix)
        {
            using (var r = new CryptoRandom(CharacterSets.AlphaNumeric))
            {
                return prefix + "-" + r.GenerateString(10);
            }
        }
    }
}