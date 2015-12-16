
using System;
using System.Reflection;

namespace CSC578_Project
{
    public class LogicExpressionHandler
    {

        public static bool EvaluateCurrentGameObject(ExpressionSet expression, GameObject gameObject)
        {
            if (expression.IsLeftSideKeyParser)
            {
                var keyParser = (KeyParser)expression.LeftSide;
                if (keyParser.PartialKeyTerms == "current")
                {
                    return SetGameObjectProperty(gameObject, keyParser.Value, expression);   
                }
            }
            return false;

        }

        public static bool SetGameObjectProperty(GameObject gameObject, string property, ExpressionSet expression)
        {
            if (gameObject.GetType().GetProperty(property) != null)
            {
                var propertyInfo = gameObject.GetType().GetProperty(property);
                if (propertyInfo.GetType().DeclaringType == expression.TypeUsed.DeclaringType)
                {
                    propertyInfo.SetValue(gameObject, expression.RightSide, null);
                    return true;
                }
            }
            return false;
        }

        public static object GetGameObjectProperty(GameObject gameObject, string property)
        {
            if (gameObject.GetType().GetProperty(property) != null)
            {
                var propertyInfo = gameObject.GetType().GetProperty(property);
                return propertyInfo.GetValue(gameObject, null);
            }
            return null;
        }

    }
}
