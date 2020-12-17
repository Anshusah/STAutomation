﻿using Cicero.Service.Helpers;
using Cicero.Service.Library;
using Cicero.Service.Library.Toastr;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Cicero.Service.Models.Component
{
    [ViewComponent(Name = "NToastNotify")]
    public class NToastNotifyViewComponent : ViewComponent
    {
        private readonly IToastNotification _toastNotification;
        private readonly ILibrary _library;
        private readonly NToastNotifyOption _nToastNotifyOption;

        public NToastNotifyViewComponent(IToastNotification toastNotification, ILibrary library, NToastNotifyOption nToastNotifyOption)
        {
            _toastNotification = toastNotification;
            _library = library;
            _nToastNotifyOption = nToastNotifyOption;
        }

        public IViewComponentResult Invoke()
        {
            var model = new ToastNotificationViewModel
            {
                ToastMessagesJson = JsonOrUndefined(_toastNotification.ReadAllMessages()),
                ResponseHeaderKey = Constants.ResponseHeaderKey,
                RequestHeaderKey = Constants.RequestHeaderKey,
                LibraryDetails = _library,
                DisableAjaxToasts = _nToastNotifyOption.DisableAjaxToasts,
                LibraryJsPath = $"/_content/{this.GetType().Assembly.GetName().Name}/{_library.VarName}.js?something"
            };
            return View("Default", model);
        }

        public string JsonOrUndefined(object obj)
        {
            return obj == null ? "undefined" : obj.ToJson();
        }
    }
}
