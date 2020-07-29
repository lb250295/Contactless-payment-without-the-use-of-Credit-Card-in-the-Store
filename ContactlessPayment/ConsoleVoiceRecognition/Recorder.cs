using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleVoiceRecognition
{
    public static class Recorder
    {
        // Indicate whether the asynchronous emulate recognition  
        // operation has completed.  
        public static bool completed;

        public static void AddPassGrammar(SpeechRecognizer recognizer)
        {
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;
            defaultDictationGrammar.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(
                NameSpeechRecognized);

            recognizer.LoadGrammar(defaultDictationGrammar);

        }

        // Handle the SpeechRecognized event of the name grammar.  
        public static void NameSpeechRecognized(
          object sender, SpeechRecognizedEventArgs e)
        {

            //Console.WriteLine("Grammar ({0}) recognized speech: {1}",
            //  e.Result.Grammar.Name, e.Result.Text);

            try
            {
                RecognizedAudio audio = e.Result.Audio;

                // Add code to verify and persist the audio.  
                string path = @"C:\temp\passwordAudio.wav";
                using (Stream outputStream = new FileStream(path, FileMode.Create))
                {
                    //RecognizedAudio passwordAudio = audio.GetRange(start, duration);
                    RecognizedAudio passwordAudio = audio;
                    passwordAudio.WriteToWaveStream(outputStream);
                    outputStream.Close();
                }

                Thread testThread =
                  new Thread(new ParameterizedThreadStart(TestAudio));
                testThread.Start(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown while processing audio:");
                Console.WriteLine(ex.ToString());
            }
        }

        // Use the speech synthesizer to play back the .wav file  
        // that was created in the SpeechRecognized event handler.  

        public static void TestAudio(object item)
        {
            string path = item as string;
            if (path != null && File.Exists(path))
            {
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                PromptBuilder builder = new PromptBuilder();
                builder.AppendText("Your recording is");
                builder.AppendAudio(path);
                synthesizer.Speak(builder);
                completed = true;
            }
        }
    }
}
