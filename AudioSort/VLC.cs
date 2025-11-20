using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSort
{
    public class VLC
    {

        private static string _port = "9091"; // input / codecs > [network settings] > 'http server port'
        private static string _user = ""; // ?
        private static string _password = "password"; // interface > main interface > lua > [lua http] > 'password'

        public static void Set (string port, string user, string pass)
        {
            _port = port?.Trim() ?? string.Empty;
            _user = user?.Trim() ?? string.Empty;
            _password = pass?.Trim() ?? string.Empty;
        }

        public static string GetPlaylistJson()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                //string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
             
                string encoded = GetUserPasswordEncoded();
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                //var b = Convert.FromBase64String("OnBhc3N3b3Jk");
                //var s = Encoding.GetEncoding("ISO-8859-1").GetString(b);

                var task = Task.Run(() => client.GetAsync($@"http://127.0.0.1:{_port}/requests/playlist.json"));
                task.Wait();
                var response = task.Result;


                string json = null;
                var task2 = Task.Run(async () => json = await response.Content.ReadAsStringAsync());
                task2.Wait();

                return json;
            }
        }

        public static string GetStatusJson()
        {

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                string encoded = GetUserPasswordEncoded();
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                var task = Task.Run(() => client.GetAsync($@"http://127.0.0.1:{_port}/requests/status.json"));
                task.Wait();
                var response = task.Result;


                string json = null;
                var task2 = Task.Run(async () => json = await response.Content.ReadAsStringAsync());
                task2.Wait();

                return json;
            }
        }

        private static string GetUserPasswordEncoded()
        {
            var enc = Encoding.GetEncoding("ISO-8859-1"); //.GetString(b);
            var b = enc.GetBytes(_user + ":" + _password);

            string encoded = System.Convert.ToBase64String(b);
            return encoded;
        }
    }
}
