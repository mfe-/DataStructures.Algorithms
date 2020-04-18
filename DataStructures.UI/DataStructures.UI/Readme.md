# DataStructures.UI

![alt tag](http://i.imgur.com/4WS122c.gif)

Contains a WPF control to visualize a graph. 
Use `DataStructures.UI.sln` and set `UI\DataStructures.Demo` as start up project to work with the `GraphControl`.

- right click on the graph control to call the context menu
- Use right click / context menu / clear / to initialize a new graph instance
- Double click on the graph control to create a new vertex
- Double click on the vertex (or right click on the vertex edit) to call the underlying vertex command (see `CommandOnDoubleClick`)
- Click on the vertex to focus on it
- When the vertex is in focus, click on one of the four adorner elements and move the mouse to the vertex you want to connect
- To remove an edge, click on the edge to set the focus, press `DEL`
- To remove an vertex, click on the vertex to set the focus, press `DEL` (First delete all edges for this to work)

### Set up a VertexControl

- For a `VertexControl` you can set up a command which should be executed on a double click. (see `CommandOnDoubleClick`) 
- You can set up a `ContextMenu`
- Set up the visualisation of your vertex data with a `DataTemplate`. See next topic.

#### DataTemplate

The vertex contains a generic data property `Value`. For this property a `DataTemplate` can be configured on the ``VertexControl`` style.
Apply your `DataTemplate` to the `ItemTemplate` DP to represent the Data of the vertex individually. 


      <Style TargetType="{x:Type ui:VertexControl}">
          <Setter Property="Width" Value="100" />
          <Setter Property="Height" Value="100" />
          <Setter Property="Background" Value="#fff299" />
          <Setter Property="CommandOnDoubleClick" Value="{Binding Path=DataContext.ClickCommand,Source={x:Reference dockPanel},UpdateSourceTrigger=Default}" />
          <Setter Property="ContextMenu" Value="{StaticResource ContextMenu}" />
          <Setter Property="ItemTemplate">
              <Setter.Value>
                  <DataTemplate>
                      <StackPanel Orientation="Vertical" Background="Yellow" 
                                  VerticalAlignment="Stretch" HorizontalAlignment="Stretch"
                                  ToolTip="{Binding Path=Value.Description,UpdateSourceTrigger=PropertyChanged}">
      
                          <TextBlock Text="{Binding Path=Value.MethodNameTyp,UpdateSourceTrigger=PropertyChanged}" TextWrapping="Wrap" />
                      </StackPanel>
                  </DataTemplate>
              </Setter.Value>
          </Setter>
      </Style>