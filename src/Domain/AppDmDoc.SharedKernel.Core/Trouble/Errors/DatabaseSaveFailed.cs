using AppDmDoc.SharedKernel.Core.Abstractions.Trouble;

namespace AppDmDoc.SharedKernel.Core.Trouble.Errors;

public class DatabaseSaveFailed : MediatorError
{
    public DatabaseSaveFailed() : base($"Failed to Save Changes to Database.") { }

    public static MediatorError New() => new DatabaseSaveFailed();
}
