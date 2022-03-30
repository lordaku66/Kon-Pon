/// <summary>
/// Konpon 2021
///  
/// The format the Karuta Phrase JSON file is serilaized into when read in KarutaGame.cs
/// 
/// Stores the details of a karuta phrase including its different translations and the correct
/// answer to the phrase.
/// 
/// id - Game ID,
/// kana - Phrase where highlighted words are in Kana,
/// english - Phrase where highlighted words are in English (only visible if the word has been discovered)
/// romaji - Phrase where highlighted words are in Romaji, (only visible if the word has been discovered)/// 
/// 
/// Author: Jacques Visser
/// </summary>


[System.Serializable]
public class KarutaPhrase
{
    public int id;
    public string kana;
    public string english;
    public string romaji;
    public string answer;
}
