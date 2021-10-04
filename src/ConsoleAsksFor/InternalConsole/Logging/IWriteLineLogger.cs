using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    internal interface IWriteLineLogger
    {
        Task LogToFile(LineTypeId lineTypeId, string value);
    }
}