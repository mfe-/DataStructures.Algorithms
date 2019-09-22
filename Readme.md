# DataStructures and algorithms

A collection of basic algorithms and data structures (algodat)

[![Build status](https://ci.appveyor.com/api/projects/status/ochuuevuytt1ekin/branch/master?svg=true)](https://ci.appveyor.com/project/mfe-/get-the-solution/branch/master)

## DataStructures (Work in Progress)

Contains the current datastructures

- The graphs datastructure with it vertices and edges is implemented as "linked list". 
- The idea is that it is not neccessary to load the entire graph to execute an algorithm.
- Each vertex can save a generic `Value`. This gives you the ability to save the data.
- The graph is fully (de)serializeable.
- LinkedList (will be reimplemented [see](https://github.com/mfe-/Abstract.DataStructures.Algorithms/issues/3))

## DataStructures.UI

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

Currently the `UI\DataStructures.Demo` is set up to create a state machine. See next topic.

## StateMachineEngine

- Makes use of the graph datastructure to implement something like a state machine.
- Each state can be modified (method, method parameters, assembly and so on)
- Connect states with edges

## Introduction

When taking a beginners class at the technical university of vienna, I started to 
implement and adopt some lessions from the algorithms and data structures course.
I thought it would be fun to put my recently gained knowledge into practice.

Of course some frameworks regarding this topic already exist. For example
[linked list](https://msdn.microsoft.com/en-us/library/he2s3bh7(v=vs.110).aspx) and various tree classes.
When applying these I ran into several issues like limited extensibility, missing features and all of them came from diffrent sources. Therefore I wasn't able to combine the tree classes, especially the data structure classes.

So I started to create my own algorithms and data structures "framework".

## Goal

With the old version of the framework I mainly focused on visually representing the graph data structure with wpf. (see `UI\DataStructures.Demo`)
So I neglected some oop aspects.
With this repository I want to restart the project and focus on:

* Reusability - data structures should be extendable and combineable
* Stability - Code should be stable enough to be run on productive systems

## Graph control

When creating the graph with the ui (graph control) the proper model will be created in the background. 
![alt tag](http://i.imgur.com/4WS122c.gif)

Another approach would be to overgive a graph data structure to the graph visualization control which created the proper ui graph.
One of the benefits of the old implemention is that the kruskal algorithm creats a copy of the graph, therefore the vertices dont't stay in the same position.

![alt tag](http://i.imgur.com/6KQueHc.gif)

When modifying a graph during runtime, the graph visualization control updates the ui. 

![alt tag](http://i.imgur.com/M1YcpDV.gif)

The aforementioned features (graph visualization control) will be implemented into the new framework as well.
