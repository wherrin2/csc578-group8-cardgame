
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CSC578_Project
{

    public static class JsonParser
    {
        private static List<GameObject> gameObjects;
        private static List<LogicObject> logicObjects; 

        public static GamePackageMeta DeserializeMeta(string json)
        {
            GamePackageMeta meta = null;
            try
            {
                meta = JsonConvert.DeserializeObject<GamePackageMeta>(json);
            }
            catch (JsonException e)
            {
                Debug.WriteLine((e.StackTrace));
            }
            return meta;
        }
        public static List<GameObject> Deserialize(string json, string fileKey)
        {
            gameObjects = new List<GameObject>();
            try
            {
                ParseToObjects(JToken.Parse(json), fileKey);
            }
            catch (JsonException e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return gameObjects;
        }

        public static List<LogicObject> DeserializeLogic(string json, string fileKey)
        {
            logicObjects = new List<LogicObject>();
            try
            {
                ParseToObjects(JToken.Parse(json), fileKey);
            }
            catch (JsonException e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return logicObjects;
        } 

        private static void ParseToObjects(JToken token, string key)
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
                    else
                    {
                        //possible matches for .rules files
                        var isExpressionSet = token["expressionSet"];
                        var isEmbeddedAction = token["embeddedAction"];
                        var isRunOnce = token["runOnce"];
                        var priority = token["priority"];
                        if (isExpressionSet != null || isEmbeddedAction != null || isRunOnce != null || priority != null)
                        {
                            var logic = JsonConvert.DeserializeObject<LogicObject>(token.ToString());
                            logic.Name = key + "." + logic.Name;
                            logicObjects.Add(logic);
                        }
                    }

                    var prop = token.Children<JProperty>().ToList();
                    for (int i = 0; i < prop.Count; i++)
                    {
                        ParseToObjects(prop[i].Value, key);
                    }
                    break;
                    

                case JTokenType.Array:
                    //name =  Cards.cards.2clubs / could support linking amongst files if implemented
                    key += "." + token.Path;
                    var list = token.Children<JToken>().ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        ParseToObjects(list[i], key);
                    }
                    break;
            }
        }
    }
}
