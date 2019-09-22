using DataStructures.Demo.View;
using Prism.Commands;
using StateMachineEngine;
using System.Windows.Input;

namespace DataStructures.Demo
{
    public class Window1ViewModel : Prism.Mvvm.BindableBase
    {
        StateMachineEngine.StateMachineEngine _stateMachine = new StateMachineEngine.StateMachineEngine();
        public Window1ViewModel()
        {
            Graph = new Graph() { Directed = true };
            PropertyChanged += Window1ViewModel_PropertyChanged;
        }

        private void Window1ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (nameof(Graph).Equals(e.PropertyName) && Graph != null)
            {
                Graph.CreateVertexFunc = VertexFactory;
            }
        }

        protected IVertex<IState> VertexFactory()
        {
            return new Vertex<IState>();
        }
        private ICommand _ClickCommand;
        public ICommand ClickCommand => _ClickCommand ?? (_ClickCommand = new DelegateCommand<IVertex>(OnClickCommand));

        protected void OnClickCommand(IVertex param)
        {
            if (param != null)
            {
                StateModuleWindow moduleFunctionWindow = new StateModuleWindow();
                var moduleFunctionWindowViewModel = new StateModuleWindowViewModel();
                moduleFunctionWindowViewModel.Vertex = param;
                moduleFunctionWindow.DataContext = moduleFunctionWindowViewModel;
                moduleFunctionWindow.ShowDialog();
            }
        }

        private ICommand _RunStateMachineCommand;
        public ICommand RunStateMachineCommand => _RunStateMachineCommand ?? (_RunStateMachineCommand = new DelegateCommand<IVertex>(OnRunStateMachineCommand));

        protected void OnRunStateMachineCommand(IVertex param)
        {
            IState state = null;
            IVertex vertex = param;
            if (param is IVertex<StateModule>)
            {
                state = (param as IVertex<StateModule>).Value;
            }
            _stateMachine.Run(vertex, state);
        }

        private Graph _Graph;
        public Graph Graph
        {
            get { return _Graph; }
            set { SetProperty(ref _Graph, value, nameof(Graph)); }
        }


    }
}
