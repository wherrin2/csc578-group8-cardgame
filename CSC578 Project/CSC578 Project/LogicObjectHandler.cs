using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC578_Project
{
    public class LogicObjectHandler
    {
        private GameObject gameObject;

        private readonly LogicObject logicObject;

        public LogicObjectHandler(LogicObject logicObject)
        {
            this.logicObject = logicObject;
        }

        public LogicObjectHandler(LogicObject logicObject, GameObject gameObject)
        {
            this.logicObject = logicObject;
            this.gameObject = gameObject;
        }

        public bool ExecuteLogicObject()
        {
            if (logicObject.EmbeddedAction.Length > 0)
               return ExecuteEmbeddedAction();
            return false;
        }


        private bool ExecuteEmbeddedAction()
        {
            var embeddedAction = new EmbeddedAction(logicObject);
            if (embeddedAction.Action.Length == 0)
                return false;

            switch (embeddedAction.Action.ToLower())
            {
                case "deal":
                    return TryDealAction(embeddedAction.ParameterStrings);
                case "allowswaps":
                    return AllowSwaps();
                case "move":
                    return TryMoveAction(embeddedAction.ParameterStrings);
                case "delete":
                    return TryDeleteAction(embeddedAction.ParameterStrings);

            }
            return false;
        }


        private bool TryDeleteAction(string[] paramStrings)
        {

            if (paramStrings.Length != 3 || gameObject == null)
                return false;
 
            var keyParser = GetKeyParser(paramStrings[0]);
            var keyParserCompareValue = GetKeyParser(paramStrings[1]);
            var keyParserDeleteValue = GetKeyParser(paramStrings[2]);

           
            if (keyParser == null || keyParserCompareValue == null || keyParserDeleteValue == null)
                return false;

            var gameObjectB = GameObjectManager.GetGameObjectByKey(keyParserDeleteValue.FileKey,
                keyParserDeleteValue.Key);
            var objA = LogicExpressionHandler.GetGameObjectProperty(gameObject, keyParserCompareValue.Value);
            var objB = LogicExpressionHandler.GetGameObjectProperty(gameObjectB, keyParserDeleteValue.Value);

            var objAType = objA.GetType();
            var objBType = objB.GetType();

            if (objA == null || objB == null || objAType == null || objBType == null)
                return false;
            if (objBType != objAType)
                return false;

            if (objAType == typeof (int))
            {
                if ((int) objA == (int) objB)
                {
                    PlayingSurfaceManager.RemoveAllGameObjectsAtPosition(gameObject);
                    return true;
                }
            } else if (objAType == typeof (string))
            {
                if (((string) objA).Equals(((string) objB)))
                {
                    PlayingSurfaceManager.RemoveAllGameObjectsAtPosition(gameObject);
                    return true;
                }
            }

            return false;
        }
        private bool TryMoveAction(string[] paramStrings)
        {
            if (paramStrings.Length != 2 || gameObject == null)
                return false;
            int x, y;
            var isSuccessful = Int32.TryParse(paramStrings[0], out x);
            if (isSuccessful)
            {
                isSuccessful = Int32.TryParse(paramStrings[1], out y);
                if (isSuccessful)
                {
                    var position = new Position(x,y);

                    foreach (var expression in logicObject.ExpressionSet)
                    {
                        expression.EvaluateExpression();
                    }
                    PlayingSurfaceManager.MoveGameObjects(gameObject, position, logicObject.ExpressionSet);
                }
            }

            return isSuccessful;
        }

        private bool AllowSwaps()
        {
            PlayingSurfaceManager.AllowSwapsCreateButton();
            return true;
        }
        private bool TryDealAction(string[] paramStrings)
        {
            if (paramStrings.Length != 4)
                return false;

            var keyOriginalOwner = GetKeyParser(paramStrings[0]);   
            var keyNewOwner = GetKeyParser(paramStrings[1]);
            if (keyOriginalOwner == null || keyNewOwner == null)
                return false;

            int x, y;
            var success = Int32.TryParse(paramStrings[2], out x);
            if (success)
            {
                success = Int32.TryParse(paramStrings[3], out y);
                if (success)
                {
                    var position = new Position(x, y);
                    var gameObjectOriginal = GameObjectManager.GetGameObjectByKey(keyOriginalOwner.FileKey, keyOriginalOwner.Key);
                    var gameObjectNew = GameObjectManager.GetGameObjectByKey(keyNewOwner.FileKey, keyNewOwner.Key);
                    if (gameObjectOriginal != null && gameObjectNew != null)
                    {
                        foreach (var expression in logicObject.ExpressionSet)
                        {
                            expression.EvaluateExpression();
                        }

                        PlayingSurfaceManager.MoveGameObjectsAndSetValues(gameObjectOriginal.OwnerId,
                               gameObjectNew.OwnerId, position, logicObject.ExpressionSet);
                      
                    }
                }
            }

           return success;
        }

        private KeyParser GetKeyParser(string input)
        {
            var keyParser = new KeyParser(input);
            if (keyParser.Key == null && keyParser.PartialKey == null)
                return null;
            return keyParser;
        }

    }
}
