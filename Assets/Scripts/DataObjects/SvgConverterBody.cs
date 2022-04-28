using System;

namespace DataObjects
{
    [Serializable]
    public class SvgConverterBody
    {
        public object[] Parameters;
    }

    [Serializable]
    public class SvgConverterFileParameter
    {
        public string Name;
        public SvgConverterFileValue FileValue;
    }
    
    [Serializable]
    public class SvgConverterOptionParameter
    {
        public string Name;
        public bool Value;
    }
    
    [Serializable]
    public class SvgConverterFileValue
    {
        public string Url;
    }
}