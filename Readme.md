# Get.the.solution
A collection of basic algorithms and data structures (algodat)

[<img src="https://ci.appveyor.com/api/projects/status/github/mfe-/Get.the.solution?branch=master&svg=true">](https://ci.appveyor.com/project/mfe-/get-the-solution)

## Introduction
When taking a begginers class at the technical university of vienna, i started to 
implement and adopt some stuff from the algorithms and data structures course.
I thought it would be fun to put my recently gained knowledge into practice.

Of course there already exist some material regarding the topic for example the
[linked list](https://msdn.microsoft.com/en-us/library/he2s3bh7(v=vs.110).aspx) and various tree classes.
When using them I ran into several issues like limited extensibility, missing features and all of them came from diffrent sources. Therefore I wasn't able to combine the tree classes, especially the data structure classes.

So I started to create my own algorithms and data structures "framework".

## Goal
With the old version (not published on github) of the framework I mainly focused on visually representing the graph data structure with wpf.
So I neglected some oop aspects.
With this repository I want to restart the project and focus on:

* Reusability - data structures should be extendable and combineable
* Universal Windows Platform - Use portable classes
* Stability - Code should be stable enough to be run on productive systems

## Graph visualization control

When creating the graph with the ui (graph visualization control) the proper model will be created in the background. 
![alt tag](http://i.imgur.com/4WS122c.gif)

Another approach would be to overgive a graph data structure to the graph visualization control which created the proper ui graph.
One of the benefits of the old implemention are that the kruskal algorithm creats a copy of the graph, therefore the vertices dont't stay on the same position.

![alt tag](http://i.imgur.com/6KQueHc.gif)

When modifying a graph during runtime, the graph visualization control updates the ui. 

![alt tag](http://i.imgur.com/M1YcpDV.gif)

The aforementioned features (graph visualization control) will be implemented into the new framework as well.