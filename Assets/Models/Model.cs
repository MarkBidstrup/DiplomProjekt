using System;
using System.Collections.Generic;


// Holds model data.
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

