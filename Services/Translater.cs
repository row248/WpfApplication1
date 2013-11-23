using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Windows.Documents;
using System.Windows.Media;
using System.Web.Script.Serialization;
using System.Collections;

using System.Runtime.Serialization.Json;
using System.Windows;

namespace LFS.Services
{
    public delegate void TranslateEventHandler(object sender, TranslateArgs args);

    public class TranslateArgs : EventArgs
    {
        private FlowDocument translate = new FlowDocument();

        public TranslateArgs(FlowDocument fd)
        {
            translate = fd; 
        }

        public FlowDocument GetTranslate()
        {
            return translate;
        }
    }

    class Translater
    {
        public event TranslateEventHandler GotTranslate;

        private WebClient wc = new WebClient();
        public MediaPlayer mp = new MediaPlayer();
        
        public Translater()
        {
            wc.DownloadStringCompleted += FormatTranslate;
        }

        public void Translate(string word)
        {
            Uri url = new Uri("http://www.translate.google.com/translate_a/t?client=x&text=" + word + "&hl=en&sl=en&tl=ru");
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.DownloadStringAsync(url);
        }

        public void Pronouncing(string word)
        {
            Uri url = new Uri("http://www.translate.google.com/translate_tts?ie=UTF-8&q=" + word + "&tl=en");
            mp.Stop();
            mp.Open(url);
            mp.Play();
        }

        
        void FormatTranslate(object sender, DownloadStringCompletedEventArgs e)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var dict = ser.Deserialize<Dictionary<string, dynamic>>(e.Result);
            FlowDocument doc = new FlowDocument();

            doc.Blocks.Add(new Paragraph(new Run(dict["sentences"][0]["trans"]))
            {
                FontSize = 18,
                Margin = new Thickness(0),
                BorderBrush = Brushes.Black,
                FontWeight = FontWeights.Medium
            });

            if (dict.ContainsKey("dict"))
            {
                foreach (var item in dict["dict"])
                {
                    Span span = new Span();

                    span.Inlines.Add(new Run(item["pos"] + Environment.NewLine)
                    {
                        FontSize = 14,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush(Color.FromRgb(80, 80, 80))

                    });

                    string translates = "";
                    foreach (var tr in item["terms"])
                    {
                        translates += tr + ", ";
                    }
                    // Remove last comma
                    span.Inlines.Add(new Run(translates.Remove(translates.Length - 2))
                        {
                            FontSize = 14,
                            Foreground = new SolidColorBrush( Color.FromRgb(60,60,60) )
                        });

                    doc.Blocks.Add(new Paragraph(span));
                }
            }

            GotTranslate(this, new TranslateArgs(doc));
        }
    }
}
