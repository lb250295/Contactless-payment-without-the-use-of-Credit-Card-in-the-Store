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
    class Program2
    {

        // Indicate whether the asynchronous emulate recognition  
        // operation has completed.  
        static bool completed;
        static Stream recorded;

        static void Main_prev(string[] args)
        {

            // Initialize an instance of the shared recognizer.  
            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {



                //GrammarBuilder


                // Create and load a sample grammar.  
                

                recognizer.LoadGrammar(/*testGrammar*/GetGrammar());
                //grammar.SetDictationContext("How do you", null);

                // Attach event handlers for recognition events.  
                recognizer.SpeechRecognized +=
                    new EventHandler<SpeechRecognizedEventArgs>(
                    SpeechRecognizedHandler);
                //recognizer.EmulateRecognizeCompleted +=
                //    new EventHandler<EmulateRecognizeCompletedEventArgs>(
                //    EmulateRecognizeCompletedHandler);

                completed = false;

                // Start asynchronous emulated recognition.   
                // This matches the grammar and generates a SpeechRecognized event.  
                //recognizer.EmulateRecognizeAsync("testing testing");

                // Wait for the asynchronous operation to complete.  
                while (!completed)
                {
                    Thread.Sleep(333);
                }

                completed = false;

                // Start asynchronous emulated recognition.  
                // This does not match the grammar or generate a SpeechRecognized event.  
                //recognizer.EmulateRecognizeAsync("testing one two three");

                // Wait for the asynchronous operation to complete.  
                while (!completed)
                {
                    Thread.Sleep(333);
                }
                //}

                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static Grammar GetGrammar()
        {
            Choices passTypes = new Choices(new string[] { "abcd", "password", "abc" });
            GrammarBuilder passwordMenu = new GrammarBuilder("My password is");
            passwordMenu.Append(passTypes);
            Grammar passwordMenuGrammar = new Grammar(passwordMenu);
            return passwordMenuGrammar;

            //var grammarBuilder = new GrammarBuilder("My password is");
            //grammarBuilder.AppendDictation();
            //Grammar testGrammar =
            //    new Grammar(grammarBuilder);
            //testGrammar.Name = "Test Grammar";
            //return testGrammar;
        }


        // Handle the SpeechRecognized event.  
        static void SpeechRecognizedHandler(
            object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                Console.WriteLine("Recognition result = {0}, Confidence = {1}",
                    e.Result.Text ?? "<no text>", e.Result.Confidence);
                ////recorded = new StreamWriter();
                //e.Result.Audio.WriteToAudioStream(recorded);

            }
            else
            {
                Console.WriteLine("No recognition result");
            }
        
        }

        static void EmulateRecognizeCompletedHandler(
            object sender, EmulateRecognizeCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                Console.WriteLine("No result generated.");
            }

            // Indicate the asynchronous operation is complete.  
            completed = true;
        }

        private SpeechRecognitionEngine LoadDictationGrammars()
        {

            // Create a default dictation grammar.  
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            // Create the spelling dictation grammar.  
            DictationGrammar spellingDictationGrammar =
              new DictationGrammar("grammar:dictation#spelling");
            spellingDictationGrammar.Name = "spelling dictation";
            spellingDictationGrammar.Enabled = true;

            // Create the question dictation grammar.  
            DictationGrammar customDictationGrammar =
              new DictationGrammar("grammar:dictation");
            customDictationGrammar.Name = "question dictation";
            customDictationGrammar.Enabled = true;

            // Create a SpeechRecognitionEngine object and add the grammars to it.  
            SpeechRecognitionEngine recoEngine = new SpeechRecognitionEngine();
            recoEngine.LoadGrammar(defaultDictationGrammar);
            recoEngine.LoadGrammar(spellingDictationGrammar);
            recoEngine.LoadGrammar(customDictationGrammar);

            // Add a context to customDictationGrammar.  
            customDictationGrammar.SetDictationContext("How do you", null);

            return recoEngine;
        }
    }
}
