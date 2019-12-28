# DataStructures and algorithms

A collection of basic algorithms and data structures (algodat)

[![Build status](https://ci.appveyor.com/api/projects/status/ochuuevuytt1ekin/branch/master?svg=true)](https://ci.appveyor.com/project/mfe-/get-the-solution/branch/master)

## DataStructures (Work in Progress - Experimental)

Contains the current datastructures

- The graphs datastructure with it vertices and edges is implemented as "linked list". 
  - The idea is that it is not neccessary to load the entire graph to execute an algorithm.
  - Each vertex can save a generic `Value`. This gives you the ability to save the data.
  - The graph is fully (de)serializeable.
- LinkedList (will be reimplemented [see](https://github.com/mfe-/Abstract.DataStructures.Algorithms/issues/3))

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

## Graph control

When creating the graph with the ui (graph control) the proper model will be created in the background. 
![alt tag](http://i.imgur.com/4WS122c.gif)

Another approach would be to overgive a graph data structure to the graph visualization control which created the proper ui graph.
One of the benefits of the old implemention is that the kruskal algorithm creats a copy of the graph, therefore the vertices dont't stay in the same position.

![alt tag](http://i.imgur.com/6KQueHc.gif)

When modifying a graph during runtime, the graph visualization control updates the ui. 

![alt tag](http://i.imgur.com/M1YcpDV.gif)

The aforementioned features (graph visualization control) will be implemented into the new framework as well.
