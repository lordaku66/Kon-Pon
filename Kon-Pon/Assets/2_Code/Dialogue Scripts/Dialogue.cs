
/// <summary>
/// KonPon 2021
///   
/// This is the structure the dialogue JSON file is Serialized into.
/// KonPon's Dialogue is stored as separate arrays in the raw JSON file which can be read as
/// Dialogue[] by Unity.
/// 
/// Author: Jacques Visser
/// </summary>

[System.Serializable]
public class Dialogue
{
    public int id;
    public string message;
    public bool questTrigger;
    public int karutaGame;
    public string newSpeaker;
}