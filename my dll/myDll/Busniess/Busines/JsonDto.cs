using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace M.Busines
{
    public class JsonDto
    {
        public string action { get; set; }

        public JsonDtoItem[] items { get; set; }
    }

    public class JsonDtoItem
    {
        public string key{get;set;}
        public string value{get;set;}

        public JsonDto[] children { get; set; }
    }

    public static class JsonDtoHelper
    {
        public static string GetJsonString(JsonDto dto)
        {
            return JsonConvert.SerializeObject(dto, typeof(JsonDto), new JsonSerializerSettings() {});
        }

        public static JsonDto ToObject(string jsonstr)
        {
            JsonDto dto = JsonConvert.DeserializeObject<JsonDto>(jsonstr);
            if (dto == null)
            {
                dto = new JsonDto();
            }

            return dto;
        }
    }
}