using Enterprise.Blocksvalley.Observer;
using System.Collections.Generic;

public class CubeController : BaseBehavior
{
    private ObserverController observer;
    private Dictionary<string, string> tablePosData = new Dictionary<string, string>();
    private void Awake()
    {
        observer = ObserverController.Instance;
        observer.AddListener("ADD_TO_CELL", this, AddToCellCallback);
    }

    private void AddToCellCallback(ObserverParam param)
    {
        var cubePos = (KeyValuePair<int, string>) param.data;
        RemoveFromCell(cubePos.Key);
        tablePosData.Add(cubePos.Key.ToString(), cubePos.Value);
    }

    private void RemoveFromCell(int key)
    {
        var cubeKey = key.ToString();
        if (tablePosData.ContainsKey(cubeKey))
        {
            tablePosData.Remove(cubeKey);
        }
    }

    public bool checkPosition(string pos)
    {
        return tablePosData.ContainsValue(pos);
    }
}
