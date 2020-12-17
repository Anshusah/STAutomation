namespace Cicero.Service.Library.Toastr
{
    public interface IToastMessage
    {
        string Message { get; }
        ILibraryOptions Options { get; }
    }
}