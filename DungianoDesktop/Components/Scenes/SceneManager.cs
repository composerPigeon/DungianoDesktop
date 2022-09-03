using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components.Scenes
{
    public enum SceneType
    {
        Level,
        MainMenu,
        LevelMenu,
        DieMenu,
        PauseMenu,
        FinalMenu
    }

    public class SceneManager
    {
        private GameScene _actualScene;
        private SceneType _actualSceneType;

        private SceneBuilder _sceneBuilder;
        private int _previousLevel;

        public SceneManager(DungianoGame dungianoGame) 
        {
            _sceneBuilder = new SceneBuilder(dungianoGame);

            _actualSceneType = SceneType.MainMenu;
            _actualScene = _sceneBuilder.CreateMainMenu();
        }

        public void ChangeToLevel(int level)
        {
            _actualSceneType = SceneType.Level;
            _changeTo(_sceneBuilder.CreateLevel(level));
        }

        public void ChangeToMenu(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.MainMenu:
                    _changeTo(_sceneBuilder.CreateMainMenu());
                    RefreshLinkedList();
                    break;
                case SceneType.LevelMenu:
                    if (_actualSceneType == SceneType.LevelMenu)
                        _changeTo(_sceneBuilder.CreateLevelMenu(_sceneBuilder.NextPageMenu()));
                    else
                        _changeTo(_sceneBuilder.CreateLevelMenu(1));
                    break;
                case SceneType.DieMenu:
                    _setPreviousLevel();
                    _changeTo(_sceneBuilder.CreateDieMenu(_previousLevel));
                    break;
                case SceneType.PauseMenu:
                    _setPreviousLevel();
                    _changeTo(_sceneBuilder.CreatePauseMenu(_previousLevel));
                    break;
                case SceneType.FinalMenu:
                    _changeTo(_sceneBuilder.CreateFinishMenu());
                    break;
            }

            _actualSceneType = sceneType;
        }


        private void _changeTo(GameScene gameScene)
        {
            gameScene.PreviousScene = _actualScene;
            _actualScene = gameScene;
        }

        public void RefreshLinkedList()
        {
            _actualScene.PreviousScene = null;
        }

        public void GoBack()
        {
            _changeTo(_actualScene.PreviousScene);
        }

        private void _setPreviousLevel()
        {
            if (_actualScene.GetType() == typeof(LevelScene))
            {
                LevelScene level = (LevelScene)_actualScene;
                _previousLevel = level.GetLevel();
            }
        }

        // ==== newArchitecture ===

        public void Update(GameTime gameTime)
        {
            _actualScene.Update(gameTime);
        }

        public void Draw()
        {
            _actualScene.Draw();
        }
    }
}
