using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;
using System.Runtime.InteropServices;

namespace Brilliantech.Framwork.Utils.ConfigUtil
{
    public class ConfigUtil
    {
        private IConfig config;
        private IConfigSource source;

   
        public ConfigUtil()
        {
        }

        public ConfigUtil(string filename)
        {
            source = new IniConfigSource(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
        }

        public ConfigUtil(string node, string filename)
        {
            source = new IniConfigSource(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename));
            config = source.Configs[node];
        }

        public List<string> Notes() {
            List<string> notes = new List<string>();
            foreach (IConfig c in source.Configs) {
                notes.Add(c.Name);
            }
            return notes;
        }

        /// <summary>
        /// update the node
        /// </summary>
        /// <param name="node">name of node</param>
        public void Update(string node, string filename)
        {
            config = source.Configs[node];
            if (config == null)
            {
                source.AddConfig(node);
                config = source.Configs[node];
            }
            RemoveKeys();
        }

        /// <summary>
        /// get value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key,string node=null)
        {
            if (node == null)
            {
                return config.Get(key);
            }
            else {
                return source.Configs[node].Get(key);
            }
        }

        /// <summary>
        /// get all values of node
        /// </summary>
        /// <returns></returns>
        public string[] GetAllNodeValue()
        {
            return config.GetValues();
        }

        public string[] GetAllNodeKey()
        {
            return config.GetKeys();
        }


        /// <summary>
        /// set key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value,string node=null)
        {
            if (node == null)
            {
                config.Set(key, value);
            }
            else {
                source.Configs[node].Set(key, value);
            }
        }

        /// <summary>
        /// save the changes
        /// </summary>
        public void Save()
        {
            source.Save();
        }

        /// <summary>
        /// remove all keys
        /// </summary>
        private void RemoveKeys()
        {
            string[] keys = config.GetKeys();
            foreach (string key in keys)
            {
                config.Remove(key);
            }
        }
    }
}
