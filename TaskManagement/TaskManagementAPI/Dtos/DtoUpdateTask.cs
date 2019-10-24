namespace TaskManagementAPI.Dtos
{
    public class DtoUpdateTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Explanation { get; set; }
        public string ReturnDataType { get; set; }
        public string MethodName { get; set; }
        public bool IsProdcution { get; set; }
        public string FirstInputParameterDataType { get; set; }
        public string SecondInputParameterDataType { get; set; }
    }
}