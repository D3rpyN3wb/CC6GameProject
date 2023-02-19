using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CC6GameProject
{
    class BackgroundMusic
    {
        private WaveOutEvent waveOut;
        private bool playerIsAlive;

        public BackgroundMusic()
        {
            // Initialize the WaveOutEvent object
            waveOut = new WaveOutEvent();
            playerIsAlive = true;
        }

        public void PlayBackgroundMusic()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MyGame.Resources.background_music.mp3";
            Stream audioStream = assembly.GetManifestResourceStream(resourceName);

            // Create a WaveStream object from the audio stream
            WaveStream mp3Reader = new Mp3FileReader(audioStream);

            // Initialize the WaveOutEvent object with the WaveStream
            waveOut.Init(mp3Reader);

            // Play the audio in a loop
            waveOut.Play();

            while (playerIsAlive)
            {
                // Check if the player is alive
                if (CheckIfPlayerIsAlive())
                {
                    // If the player is still alive, keep playing the music
                    if (waveOut.PlaybackState == PlaybackState.Stopped)
                    {
                        waveOut.Stop();
                        audioStream.Position = 0;
                        mp3Reader = new Mp3FileReader(audioStream);
                        waveOut.Init(mp3Reader);
                        waveOut.Play();
                    }
                }
                else
                {
                    // If the player is dead, stop playing the music
                    waveOut.Stop();
                    break;
                }
            }
        }

        private bool CheckIfPlayerIsAlive()
        {
            // Implement your own logic to check if the player is alive
            // For example, you could check the player's health status or if the player has any lives left
            return true; // Change this to your actual logic
        }
    }
}
/*Assembly assembly = Assembly.GetExecutingAssembly();
Random rand = new Random();
int input = rand.Next(1, 4);
string resourceName = " ";
switch (input)
{
    case 1:
        resourceName = "CC6GameProject.Resources.SlaveKnight.mp3";
        break;
    case 3:
        resourceName = "CC6GameProject.Resources.GameMusic.mp3";
        break;
    default:
        resourceName = "CC6GameProject.Resources.GameMusic.mp3";
        break;
}
Stream audioStream = assembly.GetManifestResourceStream(resourceName);

// Create a WaveStream object from the audio stream
WaveStream mp3Reader = new Mp3FileReader(audioStream);

// Create a WaveOutEvent object and initialize it with the WaveStream
WaveOutEvent waveOut = new WaveOutEvent();
waveOut.Init(mp3Reader);

// Play the audio in a loop
while (true)
{
    if (waveOut.PlaybackState == PlaybackState.Stopped)
    {
        waveOut.Stop();
        audioStream.Position = 0;
        mp3Reader = new Mp3FileReader(audioStream);
        waveOut.Init(mp3Reader);
        waveOut.Play();
    }*/