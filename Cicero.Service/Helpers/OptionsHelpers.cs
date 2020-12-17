using Cicero.Service.Library;
using Cicero.Service.Library.Toastr;
using System;
using static Cicero.Service.Enums;

namespace Cicero.Service.Helpers
{
    internal class OptionsHelpers
    {
        public static T CastOptionTo<T>(ILibraryOptions options) where T : class, ILibraryOptions, new()
        {
            EnsureSameType<T>(options);
            var opt = (options ?? new T()) as T;
            return opt;
        }
        public static void EnsureSameType<T>(object obj) where T : new()
        {
            if (obj != null)
            {
                if (typeof(T) != obj.GetType())
                {
                    throw new InvalidCastException($"Wrong options type passed. Make sure you are passing the right toast options types. Passed options type {obj.GetType().Name}. Expected options type {typeof(T).Name}");
                }
            }
        }       
        public static ToastrOptions PrepareOptionsToastr(ILibraryOptions toastrOptions, NotificationTypesToastr type, string defaultTitle)
        {
            var options = CastOptionTo<ToastrOptions>(toastrOptions);
            options.Type = type;
            options.Title = options.Title ?? defaultTitle;
            return options;
        }
    }
}
