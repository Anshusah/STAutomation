using Core.Status.Extensions;
using Core.Status.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace Core.Status
{
    public class Status : IStatus
    {
        private ITempDataDictionary tempData;

        public Status(ITempDataDictionaryFactory factory, IHttpContextAccessor contextAccessor)
        {
            tempData = factory.GetTempData(contextAccessor.HttpContext);
        }

        public void Show(string type, string message, bool dismissable = false)
        {
            var messages = tempData.Get<Queue<Message>>(Constants.Key) ?? new Queue<Message>();
            messages.Enqueue(new Message(type, message, dismissable));
            tempData.Put(Constants.Key, messages);
            var messages1 = tempData.Get<Queue<Message>>(Constants.Key);
        }
    }
}
