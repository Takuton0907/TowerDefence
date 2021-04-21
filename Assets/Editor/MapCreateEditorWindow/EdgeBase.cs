using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EdgeBase : Edge
{
    public delegate void OnConectEdgeEvent();
    event OnConectEdgeEvent _conectFin;
    public event OnConectEdgeEvent ConectFin { add { _conectFin += value; } remove { _conectFin -= value; } }

    public EdgeBase(): base()
    {
        if (output != null && input != null)
        {

            output.source = input.source;
            Debug.Log("output");
        }
    }
    /// <summary>
    /// インプットかアウトプットにポートをセットする
    /// </summary>
    /// <param name="port">セットするポート</param>
    /// <param name="inputJudge">trueでインプット</param>
    public void SetPort(Port port, bool inputJudge = true)
    {
        if (inputJudge)
        {
            input = port;
        }
        else
        {
            output = port;
        }
    }

    public override void OnPortChanged(bool isInput)
    {
        base.OnPortChanged(isInput);
        _conectFin?.Invoke();
    }
}