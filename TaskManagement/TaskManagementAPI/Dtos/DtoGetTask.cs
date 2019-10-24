using System.Collections.Generic;

namespace TaskManagementAPI.Dtos
{
    public class DtoGetTask
    {
        public int TaskNum { get; set; }
        public string Name { get; set; }
        public string ReturnDataType { get; set; }
        public string MethodName { get; set; }
        public bool IsProdcution { get; set; }
        public string FirstInputParameterDataType { get; set; }
        public string SecondInputParameterDataType { get; set; }
        public ICollection<DtoGetCase> Cases { get; set; }
    }
}