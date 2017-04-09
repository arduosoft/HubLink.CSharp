using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubLink.Clients.Helpers;
using Newtonsoft.Json.Linq;

namespace HubLink.Clients.Classes
{
    public class RemoteDictionary : IDictionary<string, string>
    {
        private string DictionaryName { get; set; }
        private Guid ApplicationKey { get; set; }

        public RemoteDictionary( Guid applicationKey):this("MAIN",applicationKey)
        {
        }
        public RemoteDictionary(string dictionaryName, Guid applicationKey)
        {
            this.ApplicationKey = applicationKey;
            this.DictionaryName = dictionaryName;
        }

        private Dictionary<string, string> cache = new Dictionary<string, string>();



        public string this[string key]
        {
            get
            {
                string result = null;
                if (TryGetValue(key, out result))
                {
                    return result;
                }
                throw new KeyNotFoundException();
            }

            set
            {
                throw new NotImplementedException("Use UI interface to manage key and values");
            }
        }

        private string GetLocalOrRemote(Guid applicationKey, string dictionaryName, string key)
        {
            string result;
            if (!cache.TryGetValue(key,out result))
            {
                var tmp = GetRemote(applicationKey, dictionaryName, key);
                result=JObject.Parse(tmp)["ItemValue"].Value<string>();
            }
            return result;
        }

        private string GetRemote(Guid applicationKey, string dictionaryName, string key)
        {
            string baseUrl = ConfigurationManager.AppSettings["HubLink_BaseUrl"];
            string url= baseUrl+ "/api/Dictionary/?publicKey=" +applicationKey.ToString()+ "&dictionaryName="+DictionaryName+"&key="+ key;
            string result= LogHelper.RequestGET(url);
            return result;
        }

        /// <summary>
        /// Return the count of element already used and copied locally
        /// </summary>
        public int Count
        {
            get
            {
                return this.cache.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.cache.Keys;
            }
        }

        public ICollection<string> Values
        {
            get
            {
                return this.cache.Values;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException("Use UI interface to manage key and values");
        }

        public void Add(string key, string value)
        {
            throw new NotImplementedException("Use UI interface to manage key and values");
        }

        /// <summary>
        /// This clear only local cache
        /// </summary>
        public void Clear()
        {
            this.cache.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            string v = null;
            TryGetValue(item.Key, out v);
            return v != null && ((item.Value != null && item.Value.Equals(v)) || (item.Value == null && v == null));
        }

        public bool ContainsKey(string key)
        {
            string v=null;
            TryGetValue(key, out v);
            return v != null;
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException("Cannot copy to array. As data are lazy cached locally we cannot make partial copy, and for performance issue we cannot download the entire dictionary");
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
         return   this.cache.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException("Use UI interface to manage key and values");
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException("Use UI interface to manage key and values");
        }

        public bool TryGetValue(string key, out string value)
        {
            var v =   GetLocalOrRemote(ApplicationKey, DictionaryName, key);
            value = null;

            if (v != null)
            {
                value = v;
                return true;
            }
            return false;
        }


        /// <summary>
        /// For performance issues this return only the already loaded elemnts
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return cache.GetEnumerator();
                
        }
    }
}
