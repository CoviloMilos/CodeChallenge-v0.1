namespace TaskManagementAPI.Dtos
{
    public class DtoGetCase
    {
        public int CaseNum { get; set; }
        public DtoGetTask Task { get; set; }
        public string FirstInputParameter { get; set; }
        public string SecondInputParameter { get; set; }
        public string ValidReturnValue { get; set; }
        public bool SecretTest { get; set; }
    }
}