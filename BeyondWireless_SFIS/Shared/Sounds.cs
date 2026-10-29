using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace BeyondWireless_SFIS.Shared
{
    public class Sounds
    {
        public static void PlaySuccess()
        {
            //VantivaPalletVerificationCS.WavFiles.error.wav
            //VantivaPalletVerificationCS.WavFiles.success.wav

            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("BeyondWireless_SFISWavFiles.success.wav"))
            {
                if (stream != null)
                {
                    SoundPlayer player = new SoundPlayer(stream);
                    player.PlaySync();
                }
            }
        }

        public static void PlayError()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("BeyondWireless_SFISWavFiles.error.wav"))
            {
                if (stream != null)
                {
                    SoundPlayer player = new SoundPlayer(stream);
                    player.PlaySync();
                }
            }
        }
    }
}
