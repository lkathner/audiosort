using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioSort
{
    /// <summary>
    /// represents a destination folder for copying and the controlling keys
    /// </summary>
    public class Crate
    {
        /// <summary>
        /// general number for the crate (determines hotkeys)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// keyboard keys that act as hotkeys for this crate
        /// </summary>
        public List<Keys> KBKeys { get; set; }
        /// <summary>
        /// textbox used as input for destination folder
        /// </summary>
        public TextBox InputControl { get; set; }
        /// <summary>
        /// destination folder
        /// </summary>
        public string FolderPath { get; set; }
        /// <summary>
        /// key name for saving destination folder to app config
        /// </summary>
        public string ConfigKey { get; set; }

        public Crate()
        {

        }

        public Crate(int id, TextBox input)
        {
            if (input != null)
            {
                input.Tag = id;
                input.Text = AppConfig.GetCrateFolder(id);
            }

            this.ID = id;
            this.InputControl = input;
            this.FolderPath = input.Text.Trim();
            this.KBKeys = GetKeysForCrate(id);
            this.ConfigKey = input.Name;
        }

        /// <summary>
        /// get keyboard key codes to match crate id
        /// </summary>
        /// <param name="crateID"></param>
        /// <returns></returns>
        private static List<Keys> GetKeysForCrate(int crateID)
        {
            return new List<Keys>() {
                crateID + Keys.D0,
                crateID + Keys.NumPad0, // keyboards with numpad keys serving as alternates when (shift)ed probably won't work with this
            };
        }
    }

    public class CrateList : List<Crate>
    {
        public void Add(int id, TextBox input)
        {
            this.Add(new Crate(id, input));
        }

        /// <summary>
        /// search the list for a crate associated to a specific keyboard key
        /// </summary>
        /// <param name="kbkey"></param>
        /// <returns></returns>
        public Crate FindByPressedKey(Keys kbkey)
        {
            var crate = this.FirstOrDefault(n => n.KBKeys.Contains(kbkey));
            return crate;
        }
    }
}
