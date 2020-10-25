using Onbox.Abstractions.V8;
using Onbox.Mvc.V8.Messaging;

namespace Onbox.GridGenerator.ContainerExtensions
{
    public static class ViewsExtensions
    {
        public static IContainer AddViews(this IContainer container)
        {
            // Adds MessageBoxService to the container
            container.AddSingleton<IMessageService, MessageBoxService>();

            return container;
        }
    }
}
