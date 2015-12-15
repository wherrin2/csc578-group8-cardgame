using System;

namespace CSC578_Project
{
    public class ExpressionSet
    {
        public string Type { get; set; }
        public Type TypeUsed { get; private set; }
        public string Expression { get; set; }
        public object RightSide { get; private set; }
        public object LeftSide { get; private set; }
        public bool IsLeftSideKeyParser { get; private set; }
        public string Operator { get; private set; }

        public bool EvaluateExpression()
        {
            if (Type.Length > 0 && SetType(Type))
            {
                if (GetOperator())
                    if (GetRightSideValue())
                        if (GetLeftSideValue())
                            return true;
            }
            return false;
        }

        private bool SetType(string type)
        {
            switch (type.ToLower())
            {
                case "bool":
                    TypeUsed = typeof(bool);
                    break;
                case "boolean":
                    TypeUsed = typeof (bool);
                    break;
                case "string":
                    TypeUsed = typeof (string);
                    break;
                case "int":
                    TypeUsed = typeof (int);
                    break;
                case "integer":
                    TypeUsed = typeof (int);
                    break;
            }
            return (Type != null) ;
        }

        private bool GetOperator()
        {
            if (Expression.Contains(">="))
                Operator = ">=";
            else if (Expression.Contains("<="))
                Operator = "<=";
            else if (Expression.Contains("="))
                Operator = "=";
            else if (Expression.Contains(">"))
                Operator = ">";
            else if (Expression.Contains("<"))
                Operator = "<";
            else if (Expression.Contains("!"))
                Operator = "!";

            return (Operator.Length > 0);
        }
        private bool GetRightSideValue()
        {
            var value = Expression.Substring(Expression.IndexOf(Operator) + 1).Trim();
            if (!value.Contains("."))
            {
                if (TypeUsed == typeof (int))
                {
                    int x;
                    var res = Int32.TryParse(value, out x);
                    if (res)
                        RightSide = x;
                    return res;

                }
                else if (TypeUsed == typeof (bool))
                {
                    bool x;
                    var res = Boolean.TryParse(value, out x);
                    if (res)
                        RightSide = x;
                    return res;
                }
            }

            return false;
        }

        private bool GetLeftSideValue()
        {
            var value = Expression.Substring(0, Expression.IndexOf(Operator)).Trim();
            if (value.Contains("."))
            {
                var keyParser = new KeyParser(value);
                if (keyParser.Key != null || keyParser.PartialKey != null)
                {
                    IsLeftSideKeyParser = true;
                    LeftSide = keyParser;
                    return true;
                }
            }
            return false;
        }
    }
}