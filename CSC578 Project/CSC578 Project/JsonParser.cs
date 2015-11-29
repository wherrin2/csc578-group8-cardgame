using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CSC578_Project
{

    public static class JsonParser
    {
        public static void Deserialize(string json)
        {
             ToGameObject(JToken.Parse(json));
        }

        private static void ToGameObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var isMovable = token["isMovable"];
                    var isDrawable = token["frontImage"];
                    var isBoundary = token["allowedOwnerIds"];
                    if (isBoundary != null)
                    {
                        var boundary = JsonConvert.DeserializeObject<BoundaryObject>(token.ToString());
                    }
                    else if (isMovable != null)
                    {
                        var movable = JsonConvert.DeserializeObject<MovableObject>(token.ToString());
                    }
                    else if (isDrawable != null)
                    {
                        
                    }

                    var prop = token.Children<JProperty>().ToList();
                    for (int i = 0; i < prop.Count; i++)
                    {
                        ToGameObject(prop[i].Value);
                    }
                    break;
                    

                case JTokenType.Array:
                    //thinking about setting the name as the complete path to the object
                    //Example in .cards file in an array 'cards' and an element named '2clubs'
                    //name =  Cards.cards.2clubs / could support linking amongst files if implemented
                    //var path = token.Path.ToString();
                    var list = token.Children<JToken>().ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        ToGameObject(list[i]);
                    }
                    break;
            }
        }
    }
}
