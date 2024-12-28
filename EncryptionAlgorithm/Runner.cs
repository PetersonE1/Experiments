using PgpCore;
using System.Drawing;

namespace EncryptionAlgorithm
{
    internal static class Runner
    {
        private static void Main()
        {
            // Console.WriteLine(EncryptionEngine.DecryptImageFileToString("dragonImage.png"));

            // PGPEncryptionEngine.GeneratePGPKeys("password");

            /*
             * Bitmap map = EncryptToBitmap("Test message, please ignore. 12345", @"C:\TEMP\Keys\public.asc");
             * Console.WriteLine(DecryptFromBitmap(map, @"C:\TEMP\Keys\public.asc", @"C:\TEMP\Keys\private.asc", "password"));
             */

            PGPEncryptionEngine engine = new(@"C:\TEMP\Keys\public.asc", @"C:\TEMP\Keys\private.asc");

            string encryptedData = engine.EncryptString("Test message, please ignore. 12345");
            Console.WriteLine(encryptedData);

            Color[] colors = ImageEncryptionEngine.StringToColors(encryptedData);
            Bitmap map = ImageEncryptionEngine.ColorsToBitmap(colors);

            map.Save("test.png");

            Color[] colors1 = ImageEncryptionEngine.BitmapToColors(map);
            string encryptedData1 = ImageEncryptionEngine.ColorsToString(colors1);
            string decryptedData = engine.DecryptString(encryptedData1, "password");
            Console.WriteLine(decryptedData);
        }

        /// <summary>
        /// Encrypts string data using PGP and encodes in a Bitmap.
        /// </summary>
        /// <param name="data">The data to be encrypted and encoded.</param>
        /// <param name="publicKeyPath">The path to a PGP public key. Must match the one used to decrypt.</param>
        /// <returns>A Bitmap encoding the encrypted data.</returns>
        public static Bitmap EncryptToBitmap(string data, string publicKeyPath)
        {
            PGPEncryptionEngine engine = new(publicKeyPath, "NULL");

            string encryptedData = engine.EncryptString(data);
            Color[] colors = ImageEncryptionEngine.StringToColors(encryptedData);
            Bitmap map = ImageEncryptionEngine.ColorsToBitmap(colors);

            return map;
        }

        /// <summary>
        /// Decrypts a PGP encoded string from an encoded Bitmap.
        /// </summary>
        /// <param name="bitmap">The Bitmap encoding the encrypted string.</param>
        /// <param name="publicKeyPath">The path to a PGP public key. Must match the one used to encrypt.</param>
        /// <param name="privateKeyPath">The path to a PGP private key. Must be paired with the public key.</param>
        /// <param name="password">The password used to generate the private key.</param>
        /// <returns>A string containing the decrypted data.</returns>
        public static string DecryptFromBitmap(Bitmap bitmap, string publicKeyPath, string privateKeyPath, string password)
        {
            PGPEncryptionEngine engine = new(publicKeyPath, privateKeyPath);

            Color[] colors = ImageEncryptionEngine.BitmapToColors(bitmap);
            string encryptedData = ImageEncryptionEngine.ColorsToString(colors);

            return engine.DecryptString(encryptedData, password);
        }
    }
}
