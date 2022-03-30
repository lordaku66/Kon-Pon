/// <summary>
/// Konpon 2021
///  
/// Collection of the KarutaPhrase objects read from the Karuta Puzzle JSON file.
/// 
/// We set the data when the JSON file is read inside KarutaGame.cs, the names of the arrays in this class must
/// match those in the JSON File.
/// 
/// We get the data when a Karuta Game is active. The 'game id' of the Karuta Game determines which
/// collection to iterate through. 
/// 
/// Author: Jacques Visser
/// </summary>


[System.Serializable]
public class Phrases
{
    public KarutaPhrase[] template;

    public KarutaPhrase[] first;
}
