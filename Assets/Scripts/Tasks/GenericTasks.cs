using System;
using Newtonsoft.Json;

namespace Game.Tasks
{
    [Serializable]
    public class GenericTask : Task
    {
        [JsonProperty("task_url")]
        public string Url;

        [JsonProperty("correct_answer")]
        public string CorrectAnswer;


        [JsonConstructor]
        public GenericTask(string id, string title, string description, string prefab, string reward, string url, string correctAnswer) : base(id, title, description, prefab, reward)
        {
            Url = url;
            CorrectAnswer = correctAnswer;
        }
    }
}