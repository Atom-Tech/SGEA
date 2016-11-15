using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace MongaTur
{
	public class pontosT
	{
        int cd;
        string id, name, desc, local;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "cdPonto")]
		public int Código
		{
			get { return cd; }
			set { cd = value;}
		}

		[JsonProperty(PropertyName = "nmPonto")]
		public string Nome
		{
			get { return name; }
			set { name = value;}
		}

		[JsonProperty(PropertyName = "dsPonto")]
		public string Desc
		{
			get { return desc; }
			set { desc = value;}
		}

        [JsonProperty(PropertyName = "localPonto")]
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

    }
}

