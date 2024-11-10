using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Model
{
    public Guid ModelId { get; }
    public string ModelName { get; }
    public List<Flag> Flags { get; }

    public Model(Guid modelId, string modelName, List<Flag> flags) 
    {
        ModelId = modelId;
        ModelName = modelName;
        Flags = flags;
    }
}

