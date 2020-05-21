using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void UpdateStackEvent();//A delegate for creating event
public class ObservableStack<T> : Stack<T>
{
    public event UpdateStackEvent OnPush;//Event that is raised when we push

    public event UpdateStackEvent OnPop;//Event that is raised when we pop something

    public event UpdateStackEvent OnClear;//Event that is raised when we clear the stack

    public ObservableStack(ObservableStack<T> items) : base(items)
    {

    }

    public ObservableStack()
    {

    }

    public new void Push(T item)
    {
        base.Push(item);

        if(OnPush != null)//Makes sure something is listening to the event before we call it
        {
            OnPush();//Calls the event
        }
    }

    public new T Pop()
    {
        T item = base.Pop();

        if(OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    public new void Clear()
    {
        base.Clear();

        if (OnClear !=null)
        {
            OnClear();
        }
    }

}