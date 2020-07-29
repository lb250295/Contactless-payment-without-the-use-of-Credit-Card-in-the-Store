using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Speech.Recognition;
using System.Threading;
using System.IO;

namespace ConsoleVoiceRecognition
{

    public class Program
    { 





        static void Main(string[] args)
        {

            // Initialize an instance of the shared recognizer.  
            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                Console.WriteLine("Please record now. the recrod will be save in C:\\temp\\passwordAudio.wav");
                Recorder.AddPassGrammar(recognizer);

                

                while (!Recorder.completed)
                {
                    Thread.Sleep(333);
                }

               

                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

        }


       
    }
}
