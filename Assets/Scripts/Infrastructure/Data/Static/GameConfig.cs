using Udar.SceneManager;
using UnityEngine;
using LogType = Infrastructure.Services.Log.Core.LogType;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Static/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Application")]
        [SerializeField] private string _androidAppKey;
        [SerializeField] private string _iosAppKey;

        [Header("Scenes")]
        [SerializeField] private SceneField _bootstrapScene;
        [SerializeField] private SceneField _mainScene;
        [SerializeField] private SceneField _tutorialScene;

        [Header("Log Preferences")]
        [SerializeField] private LogType _editorLogType = LogType.Info;
        [SerializeField] private LogType _buildLogType = LogType.Info;

        public string AppKey => Application.platform == RuntimePlatform.Android ? _androidAppKey : _iosAppKey;

        public SceneField BootstrapScene => _bootstrapScene;
        public SceneField MainScene => _mainScene;
        public SceneField TutorialScene => _tutorialScene;

        public LogType LogType => Application.isEditor ? _editorLogType : _buildLogType;
    }
}