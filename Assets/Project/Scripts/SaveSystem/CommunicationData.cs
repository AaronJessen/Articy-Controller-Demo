using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArticyVariable {
    public string Id;

    [ShowInInspector]
    public abstract string CompleteId {
        get;
    }

    public abstract ArticyVariable Copy();
}
[System.Serializable]
public class GlobalVariableState : ArticyVariable {
    public string NodeTechnicalName;
    public int MissionIndex;
    [SceneName]
    public string SceneId;
    public SceneTransitionDestination.DestinationTag SceneTag;
    //public DateTime completedDate; //Existe en el servidor
    public double CompletedTime;
    public string targetLocation;
    public int MissionAttempts;

    public override string CompleteId {
        get => $"GameState.{Id}";
    }

    public override ArticyVariable Copy() {
        return new GlobalVariableState {
            NodeTechnicalName = NodeTechnicalName,
            SceneId = SceneId,
            SceneTag = SceneTag,
            CompletedTime = CompletedTime,
            Id = Id
        };
    }

    public static SceneTransitionDestination.DestinationTag ConvertDestinationTag( string sceneTag ) {
        return (SceneTransitionDestination.DestinationTag)System.Enum.Parse( typeof( SceneTransitionDestination.DestinationTag ), sceneTag );
    }
}
[System.Serializable]
public class Item : ArticyVariable {
    public override string CompleteId {
        get => $"Items.{Id}";
    }

    public override ArticyVariable Copy() {
        return new Item {
            Id = Id
        };
    }
}
