using Avalonia;
using Avalonia.Markup.Xaml;

namespace DataStructures.UI.Demo
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}