using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ImageDataStorage;
using System.Drawing;
using PgpCore;
using System.Text;

namespace EncryptionAlgorithm
{
    public class PGPEncryptionEngine
    {
        public FileInfo PublicKeyFile { get; private set; }
        public FileInfo PrivateKeyFile { get; private set; }

        /// <summary>
        /// Initializes the PGP Encryption Engine.
        /// </summary>
        /// <param name="publicKeyPath">A path to your public key used for encryption/decryption.</param>
        /// <param name="privateKeyPath">A path to your private key for decryption. Unused while encrypting, can be given an empty path.</param>
        public PGPEncryptionEngine(string publicKeyPath, string privateKeyPath)
        {
            PublicKeyFile = new(publicKeyPath);
            PrivateKeyFile = new(privateKeyPath);
        }

        // Base Methods

        /// <summary>
        /// Generates a pair of PGP keys.
        /// </summary>
        /// <param name="password">Password for the private key.</param>
        /// <param name="publicKeyPath">Output path to the public key used for encryption and paired with the private key for decryption.</param>
        /// <param name="privateKeyPath">Output path to the private key used with the public key for decryption.</param>
        public static void GeneratePGPKeys(string password, string publicKeyPath = @"C:\TEMP\Keys\public.asc", string privateKeyPath = @"C:\TEMP\Keys\private.asc")
        {
            using (PGP pgp = new())
            {
                pgp.SymmetricKeyAlgorithm = Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Aes256;

                FileInfo publicKey = new(publicKeyPath);
                FileInfo privateKey = new(privateKeyPath);
                pgp.GenerateKey(publicKey, privateKey, password: password);
            }
        }

        /// <summary>
        /// Encrypts a string using PGP.
        /// </summary>
        /// <returns>
        /// A string containing the encrypted data.
        /// </returns>
        /// <param name="data">The data to be encrypted.</param>
        public string EncryptString(string data)
        {
            if (!PublicKeyFile.Exists)
                throw new FileNotFoundException("Public key not found.");

            EncryptionKeys encryptionKeys = new(PublicKeyFile);

            using (PGP pgp = new(encryptionKeys))
            {
                pgp.SymmetricKeyAlgorithm = Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Aes256;

                return pgp.Encrypt(data);
            }
        }

        /// <summary>
        /// Decrypts a PGP encrypted string.
        /// </summary>
        /// <returns>
        /// A string containing the decrypted data.
        /// </returns>
        /// <param name="data">The data to be decrypted.</param>
        /// <param name="password">The password used to generate the private key.</param>
        public string DecryptString(string data, string password)
        {
            if (!PublicKeyFile.Exists)
                throw new FileNotFoundException("Public key not found.");
            if (!PrivateKeyFile.Exists)
                throw new FileNotFoundException("Private key not found.");

            EncryptionKeys encryptionKeys = new(PublicKeyFile, PrivateKeyFile, password);

            using (PGP pgp = new(encryptionKeys))
            {
                pgp.SymmetricKeyAlgorithm = Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Aes256;

                return pgp.Decrypt(data);
            }
        }
    }
}