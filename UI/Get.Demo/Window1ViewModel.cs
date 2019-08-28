using DataStructures.Demo.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class Window1ViewModel : Prism.Mvvm.BindableBase
    {
        public Window1ViewModel()
        {
            Graph = new Graph() { Directed = true };
            Graph.CreateVertexFunc = VertexFactory;
        }
        protected IVertex<IModule> VertexFactory()
        {
            return new Vertex<IModule>();
        }
        private ICommand _ClickCommand;
        public ICommand ClickCommand => _ClickCommand ?? (_ClickCommand = new DelegateCommand<IVertex>(OnClickCommand));

        protected void OnClickCommand(IVertex param)
        {
            var vertex = param as IVertex<IModule>;

            ModuleFunctionWindow moduleFunctionWindow = new ModuleFunctionWindow();
            var moduleFunctionWindowViewModel = new ModuleFunctionWindowViewModel();
            moduleFunctionWindowViewModel.Vertex = vertex;
            moduleFunctionWindow.DataContext = moduleFunctionWindowViewModel;
            moduleFunctionWindow.ShowDialog();


        }
        private Graph _Graph;
        public Graph Graph
        {
            get { return _Graph; }
            set { SetProperty(ref _Graph, value, nameof(Graph)); }
        }


    }
}
