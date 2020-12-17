namespace Cicero.Service.Library.Toastr.MessageContainers
{
    public interface IMessageContainerFactory
    {
        IMessageContainer<TMessage> Create<TMessage>() where TMessage : class, IToastMessage;
    }
}
