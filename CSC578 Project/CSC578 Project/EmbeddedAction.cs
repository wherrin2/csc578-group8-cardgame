using System;


namespace CSC578_Project
{
    

    public class EmbeddedAction
    {
        public string Action { get; }
        public string[] ParameterStrings { get; }

        public EmbeddedAction(LogicObject logic)
        {
            if (logic.EmbeddedAction.Length > 0)
            {
                var delimiter = new char[] {'(', ',', ')'};
                var words = logic.EmbeddedAction.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                {
                    ParameterStrings = new string[words.Length - 1]; //first split word is the method to call
                    Action = words[0];
                    for (int i = 1; i < words.Length; i++)
                    {
                        ParameterStrings[i-1] = words[i];
                    }
                }

            }
            
        }

    }
}
