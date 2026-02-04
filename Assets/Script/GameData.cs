using System.Collections.Generic;

[System.Serializable]
public class UserProfile
{
    public string name;
    public int age;
    public string gender;
    public string job;
    public string interest;
}

[System.Serializable]
public class ChatMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class UserAction
{
    public UserProfile user_profile;
    public List<ChatMessage> history;
    public string user_input;
    public int current_risk_score;
}

[System.Serializable]
public class GameResponse
{
    public string scammer_dialogue;
    public int risk_change;
    public bool is_game_over;
    public string ending_type;
    public string emotion;
}