namespace CodeCompilerAPI.Models
{
    public class Case
    {
        public int CaseNum { get; set; }
        public Task Task { get; set; }
        public string FirstInputParameter { get; set; }
        public string SecondInputParameter { get; set; }
        public string ValidReturnValue { get; set; }
        public bool SecretTest { get; set; }
        public bool CaseResult { get; set; }
    }
}