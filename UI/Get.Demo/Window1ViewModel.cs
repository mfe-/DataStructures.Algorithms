using DataStructures.Demo.View;
using Prism.Commands;
using StateMachineEngine;
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
        protected IVertex<StateModule> VertexFactory()
        {
            return new Vertex<StateModule>();
        }
        private ICommand _ClickCommand;
        public ICommand ClickCommand => _ClickCommand ?? (_ClickCommand = new DelegateCommand<IVertex>(OnClickCommand));

        protected void OnClickCommand(IVertex param)
        {
            StateModuleWindow moduleFunctionWindow = new StateModuleWindow();
            var moduleFunctionWindowViewModel = new StateModuleWindowViewModel();
            moduleFunctionWindowViewModel.Vertex = param;
            moduleFunctionWindow.DataContext = moduleFunctionWindowViewModel;
            moduleFunctionWindow.ShowDialog();
        }

        private ICommand _RunStateMachineCommand;
        public ICommand RunStateMachineCommand => _RunStateMachineCommand ?? (_RunStateMachineCommand = new DelegateCommand<object>(OnRunStateMachineCommand));

        protected void OnRunStateMachineCommand(object param)
        {

        }

        private Graph _Graph;
        public Graph Graph
        {
            get { return _Graph; }
            set { SetProperty(ref _Graph, value, nameof(Graph)); }
        }


    }
}
