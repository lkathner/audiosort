using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSort
{
    public class Playlist
    {
        private string json;

        public Playlist(string json)
        {
            this.Load(json);
        }

        public void Load(string json)
        {
            this.json = json;

            var jser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var pl2 = (Dictionary<string, object>)jser.DeserializeObject(json);

            //var json2 = System.Web.HttpUtility.UrlDecode(json);
            var json2 = json;
            var playlist = jser.Deserialize<Root>(json2);
            var plNode = playlist.children.FirstOrDefault(c => string.Equals(c.name, "Playlist", StringComparison.OrdinalIgnoreCase));

            this.Songs = new List<Song>(plNode.children.Select(c => new Song(c)));
        }

        public List<Song> Songs { get; set; }

        public Song GetSong(string title)
        {
            if (this.Songs == null)
                return null;

            var song = this.Songs.FirstOrDefault(n => n.Location.EndsWith(title, StringComparison.OrdinalIgnoreCase));
            return song;
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Child
        {
            public string ro { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public string id { get; set; }
            public List<Child> children { get; set; }
            public int duration { get; set; }
            public string uri { get; set; }
            public string current { get; set; }
        }

        public class Root
        {
            public string ro { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public string id { get; set; }
            public List<Child> children { get; set; }
        }

        internal Song Pick(Status status)
        {
            var song = this.Songs.FirstOrDefault(n => n.Location.EndsWith(status.CurrentSong));
            return song;
        }
    }

    public class Song
    {
        private Playlist.Child c;

        public Song(Playlist.Child c)
        {
            this.c = c;

            _uri = System.Web.HttpUtility.UrlDecode(c.uri);
            this.Location = _uri.Substring(8); // remove file:///
            this.Title = c.name;


            if (!System.IO.File.Exists(this.Location))
            {
                System.Diagnostics.Debug.WriteLine($"path {this.Location} is invalid");
            }
        }

        public string Title { get; set; }
        //public string Uri { get; set; }
        public string Location { get; set; }

        public string Filename
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Location))
                    return System.IO.Path.GetFileName(this.Location);

                return null;
            }
        }

        private string _uri;
    }



}
