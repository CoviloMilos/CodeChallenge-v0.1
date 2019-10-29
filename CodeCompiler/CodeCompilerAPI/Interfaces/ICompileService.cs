namespace CodeCompilerAPI.Interfaces
{
    public interface ICompileService
    {
        Models.Task CompileCode(string inputCode, Models.Task task);
    }
}