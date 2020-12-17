using System;

namespace Core.Status
{
    public interface IStatus
    {
        void Show(string type, string message, bool dismissable = false);
    }
}
