# DataStructures and algorithms

A collection of basic algorithms and data structures (algodat)

## DataStructures (Experimental)

- AvlTree
- BstTree
- The graphs datastructure with it vertices and edges is implemented as "linked list". 
  - The idea is that it is not neccessary to load the entire graph to execute an algorithm.
  - Each vertex can save a generic `Value`.
  - The graph is fully (de)serializeable.
- LinkedList (will be reimplemented [see](https://github.com/mfe-/Abstract.DataStructures.Algorithms/issues/3))
- [Documentation](https://mfe-.github.io/DataStructures.Algorithms.Docs/)

## nugets

| nuget.org | 
| ------------- |
| [Abstract.DataStructures](https://www.nuget.org/packages/Abstract.DataStructures) |  
| [Abstract.DataStructures.Algorithms](https://www.nuget.org/packages/Abstract.DataStructures.Algorithms) | 
| [Abstract.DataStructures.Algorithms.Graph](https://www.nuget.org/packages/Abstract.DataStructures.Algorithms.Graph) |
| [Abstract.DataStructures.UI](https://www.nuget.org/packages/Abstract.DataStructures.UI) | 


| Azure DevOps      | 
| ------------- |
| [Pipeline](https://dev.azure.com/get-the-solution/get-the-solution/_build?definitionId=9) |
| [Doc Release Pipeline](https://dev.azure.com/get-the-solution/get-the-solution/_release?_a=releases&view=mine&definitionId=3) |

## DataStructures.UI

Wpf control to visualize, create and edit a graph. See [here](https://github.com/mfe-/DataStructures.Algorithms/blob/main/docs/articles/GraphControl.md).

## Live Samples

### A* search algorithm
![alt text](https://raw.githubusercontent.com/mfe-/Abstract.DataStructures.Algorithms/master/astar.gif)

### BFS - breadth first search
![alt text](https://raw.githubusercontent.com/mfe-/Abstract.DataStructures.Algorithms/master/BreadthFirstSearch.gif)

### Kruskal
![alt text](https://raw.githubusercontent.com/mfe-/Abstract.DataStructures.Algorithms/master/kruskal.gif)

### Introduction

When taking a beginners class at the technical university of vienna, I started to 
implement and adopt some lessions from the algorithms and data structures course.
I thought it would be fun to put my recently gained knowledge into practice.

Of course some frameworks regarding this topic already exist. For example
[linked list](https://msdn.microsoft.com/en-us/library/he2s3bh7(v=vs.110).aspx) and various tree classes.
When applying these I ran into several issues like limited extensibility, missing features and all of them came from diffrent sources. Therefore I wasn't able to combine the tree classes, especially the data structure classes.

So I started to create my own algorithms and data structures "framework".

#### Goal

With the old version of the framework I mainly focused on visually representing the graph data structure with wpf. (see `UI\DataStructures.Demo`)

##### Some algo related links

- [The Algorithm Design Manual](https://link.springer.com/book/10.1007%2F978-1-84800-070-4)
- A*Star https://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/article.php/c12527/AStar-A-Implementation-in-C-Path-Finding-PathFinder.htm
- The Travelling salesman problem is one of the most well know NP-hard problem. Concorde’s solver can be used to solve exactly or approximately even large instances. <http://www.math.uwaterloo.ca/tsp/index.html>
- PRIM <http://bioinfo.ict.ac.cn/~dbu/AlgorithmCourses/Lectures/Prim1957.pdf>

#### Graph control

When creating the graph with the ui (graph control) the proper model will be created in the background. 
![alt tag](http://i.imgur.com/4WS122c.gif)

Another approach would be to overgive a graph data structure to the graph visualization control which created the proper ui graph.
One of the benefits of the old implemention is that the kruskal algorithm creats a copy of the graph, therefore the vertices dont't stay in the same position.

![alt tag](http://i.imgur.com/6KQueHc.gif)

When modifying a graph during runtime, the graph visualization control updates the ui. 

![alt tag](http://i.imgur.com/M1YcpDV.gif)

The aforementioned features (graph visualization control) will be implemented into the new framework as well.
