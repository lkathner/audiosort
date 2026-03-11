using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSort
{
    public class Status
    {
        const string PLAYING = "playing";
        const string PAUSED = "paused";
        const string STOPPED = "stopped";

        private string json;

        public string State { get; set; }
        public string CurrentSong { get; set; }
        public int Time { get; set; }
        public int Length { get; set; }

        public bool IsPlaying
        {
            get { return string.Equals(this.State, PLAYING); }
        }

        public Status(string json = null)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                this.Load(json);
            }
        }

        public void Load(string json2)
        {

            this.json = json2;

            var jser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var status = jser.Deserialize<Root>(json2);

            this.State = status?.state;
            this.CurrentSong = status.information?.category?.meta?.filename?.Trim();
            this.Time = status.time;
            this.Length = status.length;

        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Audiofilters
        {
            public string filter_0 { get; set; }
        }

        public class Category
        {
            public Meta meta { get; set; }

            //[JsonProperty("Stream 0")]
            //public Stream0 Stream0 { get; set; }
        }

        public class Information
        {
            public int chapter { get; set; }
            public List<object> chapters { get; set; }
            public int title { get; set; }
            public Category category { get; set; }
            public List<object> titles { get; set; }
        }

        public class Meta
        {
            public string track_total { get; set; }
            public string date { get; set; }
            public string encoded_by { get; set; }
            public string artist { get; set; }
            public string album { get; set; }
            public string track_number { get; set; }
            public string filename { get; set; }
            public string title { get; set; }
            public string genre { get; set; }
        }

        public class Root
        {
            public int fullscreen { get; set; }
            public Stats stats { get; set; }
            public int seek_sec { get; set; }
            public int apiversion { get; set; }
            public int currentplid { get; set; }
            public int time { get; set; }
            public int volume { get; set; }
            public int length { get; set; }
            public bool random { get; set; }
            public Audiofilters audiofilters { get; set; }
            public Information information { get; set; }
            public int rate { get; set; }
            public Videoeffects videoeffects { get; set; }
            public string state { get; set; }
            public bool loop { get; set; }
            public string version { get; set; }
            public double position { get; set; }
            public int audiodelay { get; set; }
            public bool repeat { get; set; }
            public int subtitledelay { get; set; }
            public List<object> equalizer { get; set; }
        }

        public class Stats
        {
            public double inputbitrate { get; set; }
            public int sentbytes { get; set; }
            public int lostabuffers { get; set; }
            public int averagedemuxbitrate { get; set; }
            public int readpackets { get; set; }
            public int demuxreadpackets { get; set; }
            public int lostpictures { get; set; }
            public int displayedpictures { get; set; }
            public int sentpackets { get; set; }
            public int demuxreadbytes { get; set; }
            public double demuxbitrate { get; set; }
            public int playedabuffers { get; set; }
            public int demuxdiscontinuity { get; set; }
            public int decodedaudio { get; set; }
            public int sendbitrate { get; set; }
            public int readbytes { get; set; }
            public int averageinputbitrate { get; set; }
            public int demuxcorrupted { get; set; }
            public int decodedvideo { get; set; }
        }

        public class Stream0
        {
            public string Bitrate { get; set; }
            public string Codec { get; set; }
            public string Channels { get; set; }
            public string Bits_per_sample { get; set; }
            public string Type { get; set; }
            public string Sample_rate { get; set; }
        }

        public class Videoeffects
        {
            public int hue { get; set; }
            public int saturation { get; set; }
            public int contrast { get; set; }
            public int brightness { get; set; }
            public int gamma { get; set; }
        }

    }
}
