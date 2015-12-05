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
        private static List<GameObject> gameObjects;
        public static List<GameObject> Deserialize(string json, string fileKey)
        {
            gameObjects = new List<GameObject>();
            ToGameObject(JToken.Parse(json), fileKey);
            return gameObjects;
        }

        private static void ToGameObject(JToken token, string key)
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
                        boundary.Name = key + "." + boundary.Name;
                        gameObjects.Add(boundary);
                    }
                    else if (isMovable != null)
                    {
                        var movable = JsonConvert.DeserializeObject<MovableObject>(token.ToString());
                        movable.Name = key + "." + movable.Name;
                        gameObjects.Add(movable);
                    }
                    else if (isDrawable != null)
                    {
                        var drawable = JsonConvert.DeserializeObject<DrawableObject>(token.ToString());
                        drawable.Name = key + "." + drawable.Name;
                        gameObjects.Add(drawable);
                    }

                    var prop = token.Children<JProperty>().ToList();
                    for (int i = 0; i < prop.Count; i++)
                    {
                        ToGameObject(prop[i].Value, key);
                    }
                    break;
                    

                case JTokenType.Array:
                    //Example in .cards file in an array 'cards' and an element named '2clubs'
                    //name =  Cards.cards.2clubs / could support linking amongst files if implemented
                    key += "." + token.Path;
                    var list = token.Children<JToken>().ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        ToGameObject(list[i], key);
                    }
                    break;
            }
        }
    }
}
