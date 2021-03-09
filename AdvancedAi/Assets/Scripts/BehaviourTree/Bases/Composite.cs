////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Composite.cs 
///Created by: Charlie Bullock based on behaviour tree example given in CT6024
///Description: This class contains a lit of child nodes which can be added to via the AddChild function
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Inherits from node
public class Composite : Node
{
    //Variables
    private List<Node> childNodes;
    protected int currentIndex = 0;

    //Function returns list of child nodes when called
    public List<Node> GetChildBehaviours()
    {
        return childNodes;
    }

    //Function adds node to the child nodes list that is passed in as parameter
    public void AddChild(Node newChild)
    {
        childNodes.Add(newChild);
    }

    //Function resets current index
    protected void Reset()
    {
        currentIndex = 0;
    }

    //Function sets index to 0 then sets child nodes as new list
    protected void Init()
    {
        currentIndex = 0;
        childNodes = new List<Node>();
    }
}
