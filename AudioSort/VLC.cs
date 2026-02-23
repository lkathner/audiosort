using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSort
{
    public class VLC
    {

        private static string _host = "127.0.0.1"; // todo
        private static string _port = "9091"; // input / codecs > [network settings] > 'http server port'
        private static string _user = ""; // ?
        private static string _password = "password"; // interface > main interface > lua > [lua http] > 'password'

        public static void Set(string port, string user, string pass)
        {
            _port = port?.Trim() ?? string.Empty;
            _user = user?.Trim() ?? string.Empty;
            _password = pass?.Trim() ?? string.Empty;
        }

        private static async Task<string> GetPlaylistJson2()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                string encoded = GetUserPasswordEncoded();
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded); // todo use the auth enum instead of string

                var response = await client.GetAsync($@"http://127.0.0.1:{_port}/requests/playlist.json");
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }

        private static async Task<string> GetStatusJson()
        {

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                string encoded = GetUserPasswordEncoded();
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                var response = await client.GetAsync($@"http://127.0.0.1:{_port}/requests/status.json");
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }

        private static string GetUserPasswordEncoded()
        {
            //string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            //var b = Convert.FromBase64String("OnBhc3N3b3Jk");
            //var s = Encoding.GetEncoding("ISO-8859-1").GetString(b);

            var creds = $"{_user}:{_password}";

            var enc = Encoding.GetEncoding("ISO-8859-1"); //.GetString(b);
            var b = enc.GetBytes(creds);

            string encoded = System.Convert.ToBase64String(b);
            return encoded;
        }

        private static Exception WrapException(Exception ex)
        {
            var x = ex;
            while (x != null)
            {
                if ((uint)x.HResult == 0x80004005) 	  // connection refused
                {
                    var msg = "VLC not running";
                    throw new Exception(msg, ex);
                }

                x = x.InnerException;
            }

            return ex;
        }


        public static async Task<Playlist> GetPlaylistInfo()
        {
            try
            {
                var json = await GetPlaylistJson2();
                var pl3 = new Playlist(json);
                return pl3;
            }
            catch (Exception ex)
            {
                var ex2 = WrapException(ex);
                if (ex2 != ex)
                {
                    throw ex2;
                }

                throw;
            }
        }

        public static async Task<Status> GetCurrentStatus()
        {
            try
            {
                var json = await VLC.GetStatusJson();
                var status = new Status(json);
                return status;
            }
            catch (Exception ex)
            {
                var ex2 = WrapException(ex);
                if (ex2 != ex)
                {
                    throw ex2;
                }

                throw;
            }
        }
    }
}
