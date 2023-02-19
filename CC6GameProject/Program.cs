using NAudio.Wave;
using System;
using System.IO;
using System.Media;
using System.Reflection;

namespace CC6GameProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "CC6GameProject.Resources.GigaChad_8bit_Song.mp3";
            string buttonsound1 = "CC6GameProject.Resources.GigaChad_8bit_Song.mp3";
            Stream audioStream = assembly.GetManifestResourceStream(resourceName);
            Stream abuttonsound1 = assembly.GetManifestResourceStream(buttonsound1);
            // Create a WaveStream object from the audio stream
            WaveStream mp3Reader = new Mp3FileReader(audioStream);

            // Create a WaveOutEvent object and initialize it with the WaveStream
            WaveOutEvent waveOut = new WaveOutEvent();
            waveOut.Init(mp3Reader);

            // Play the audio in a loop
            waveOut.Play();
            while (true)
            {
                if (waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    waveOut.Stop();
                    audioStream.Position = 0;
                    mp3Reader = new Mp3FileReader(audioStream);
                    waveOut.Init(mp3Reader);
                    waveOut.Play();
                }



                Console.OutputEncoding = System.Text.Encoding.GetEncoding(28591);
                Console.CursorLeft = Console.BufferWidth / 2 - 10 / 2;
                Console.WriteLine("Rogueish Rogue");
                Console.WriteLine("========================================================================================================================");
                string introText = @"Welcome to the world of Ar'rakrin, where you will embark on an epic adventure through dark and treacherous dungeons filled with untold treasures, dangerous monsters, and mysterious magical scrolls. As a brave adventurer seeking fortune and glory, your task is to navigate the dungeon and collect as many coins as possible while avoiding deadly traps and ferocious enemies that lurk around every corner.

In this exciting rogue-like game, you will explore the depths of the dungeon, uncover hidden treasures, and battle dangerous monsters using an array of powerful weapons and spells. Along the way, you will encounter magical scrolls that can aid you in your quest, but be warned - not all scrolls are friendly!

With each coin you collect, you will become stronger and more capable, allowing you to venture deeper into the dungeon and face even greater challenges. Will you be able to survive the perils of the dungeon and emerge victorious, or will you fall to the many dangers that await within? Only time will tell in the world of Rogue!";
                string loreText = @"Deep in the heart of the kingdom lies a long-forgotten dungeon, rumored to be filled with untold treasures and priceless artifacts. For years, adventurers have sought to brave its depths, hoping to emerge with wealth beyond their wildest dreams.

One day, you stumbled upon a map of the dungeon, and you couldn't resist the lure of the treasures within. You made your way to the dungeon and, with a sense of excitement and trepidation, you pushed open the door and stepped inside.

The dungeon was as dark and foreboding as the legends had described, and you soon found yourself lost in its twisting corridors. As you searched for the treasure, you suddenly heard a loud noise and turned to see the door slamming shut behind you. The dungeon had sealed you in, and you knew that your only hope was to find another way out.

But the treasures still beckoned to you, and you couldn't resist the temptation to collect as much gold as you could before you made your escape. With your trusty sword at your side and your wits about you, you set out into the depths of the dungeon, ready to face whatever dangers lay ahead. Will you be able to survive the perils of the dungeon and emerge with your life and your riches intact? Only time will tell.";

                Console.WriteLine(introText);
                Console.WriteLine("-------------------------------------------------Press Any Key To Continue----------------------------------------------");
                Console.ReadKey();

                int current_Choice = 0;
                bool done_choice = false;
                while (!done_choice)
                {
                    Console.Clear();

                    Console.WriteLine("Menu:");

                    if (current_Choice == 0)
                    {
                        Console.WriteLine("> 1. Play");
                    }
                    else
                    {
                        Console.WriteLine("  1. Play");
                    }

                    if (current_Choice == 1)
                    {
                        Console.WriteLine("> 2. Controls/Legends");
                    }
                    else
                    {
                        Console.WriteLine("  2. Controls/Legends");
                    }

                    if (current_Choice == 2)
                    {
                        Console.WriteLine("> 3. Exit");
                    }
                    else
                    {
                        Console.WriteLine("  3. Exit");
                    }

                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            current_Choice--;
                            if (current_Choice < 0)
                            {
                                current_Choice = 0;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            current_Choice++;
                            if (current_Choice > 2)
                            {
                                current_Choice = 2;
                            }
                            break;
                        case ConsoleKey.Enter:
                            switch (current_Choice)
                            {
                                case 0:
                                    Console.Clear();
                                    Console.WriteLine("========================================================================================================================");
                                    Console.WriteLine(loreText);
                                    Console.WriteLine("Starting a new dungeon...");
                                    Console.WriteLine("-------------------------------------------------Press Any Key To Continue----------------------------------------------");
                                    Console.ReadKey();
                                    waveOut.Stop();
                                    Game g = new Game(new Point(200, 40));
                                    // Code to start a new dungeon goes here
                                    break;
                                case 1:
                                    Console.WriteLine("========================================================================================================================");
                                    Console.WriteLine("Controls/Legends:");
                                    Console.WriteLine(" > (←→↑↓) Use arrow keys to move and interact with the world");
                                    Console.WriteLine(" > (⇥) Press Tab to open your Inventory");
                                    Console.WriteLine(" > (Esc) Press Escape to exit the game");

                                    Console.WriteLine(" ");
                                    Console.WriteLine(" ▲(Blue) = Starting Spawn");
                                    Console.WriteLine(" ▲(Green) = Exit Dungeon");
                                    Console.WriteLine(" * = Gold");
                                    Console.WriteLine(" ■(Yellow) = Chest");
                                    Console.WriteLine(" a = Scroll");

                                    Console.WriteLine(" ");
                                    Console.WriteLine(" r = Rat");
                                    Console.WriteLine(" S = Snake");
                                    Console.WriteLine(" G = Goblin");
                                    Console.WriteLine(" O = Orc");
                                    Console.WriteLine(" D = Dragon");


                                    // Code to display controls goes here
                                    break;
                                case 2:
                                    Console.WriteLine("========================================================================================================================");
                                    Console.WriteLine("Exiting the game...");
                                    Environment.Exit(0);
                                    done_choice = true;
                                    break;
                            }
                            Console.WriteLine();
                            Console.WriteLine("-------------------------------------------------Press Any Key To Continue----------------------------------------------");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }
    }
}
//Game g = new Game(new Point(200, 40));