using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.MenuComponents;
using DungianoDesktop.Components.Characters;

namespace DungianoDesktop.Components.Scenes
{
    public class SceneBuilder
    {
        private (int Width, int Height) _screenSize;
        private DungianoGame _dungianoGame;

        private int _maxLevelMenuPage = 2;
        private int _levelMenuPage;

        public SceneBuilder(DungianoGame dungianoGame)
        {
            _screenSize = dungianoGame.GetScreenSize();
            _dungianoGame = dungianoGame;
        }

        public MenuScene CreateMainMenu()
        {
            //create buttons
            List<Button> buttons = new List<Button>();
            // play button
            buttons.Add(new LevelNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/Play/normal", "Buttons/Play/lighter", "Buttons/Play/darker", "Buttons/Play/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 4),
                scale: 0.5f,
                level: _dungianoGame.GameData.MaxLevelVisited
                )
            );
            // level menu button
            buttons.Add(new MenuNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/Levels/normal", "Buttons/Levels/lighter", "Buttons/Levels/darker", "Buttons/Levels/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 2),
                scale: 0.5f,
                nextScene: SceneType.LevelMenu
                )
            );
            //quit button
            buttons.Add(new ClassicButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/Quit/normal", "Buttons/Quit/lighter", "Buttons/Quit/darker", "Buttons/Quit/normal"),
                position: new Vector2(_screenSize.Width / 2, 3 * _screenSize.Height / 4),
                scale: 0.5f,
                function: _dungianoGame.QuitGame
                )
            );

            return new MenuScene(_dungianoGame, buttons, new Background(_dungianoGame, "MenuBackgrounds/menuBackground"));
        }

        public MenuScene CreateLevelMenu(int page)
        {
            _levelMenuPage = page;
            //create buttons
            List<Button> buttons = new List<Button>();
            // main menu button
            buttons.Add(new MenuNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/Back/normal", "Buttons/Back/lighter", "Buttons/Back/darker", "Buttons/Back/normal"),
                position: new Vector2(200, 150),
                scale: 0.5f,
                nextScene: SceneType.MainMenu
                )
            );

            if (page < _maxLevelMenuPage)
            {
                //next button
                buttons.Add(new MenuNavigationButton(
                    dungianoGame: _dungianoGame,
                    texturesNames: ("Buttons/ArrowR/normal", "Buttons/ArrowR/lighter", "Buttons/ArrowR/darker", "Buttons/ArrowR/normal"),
                    position: new Vector2(_screenSize.Width - 200, 150),
                    scale: 0.5f,
                    nextScene: SceneType.LevelMenu
                    )
                );
            }

            _createLevelButtons(buttons, page);

            _setLevelButtonsDisabled(buttons, page);

            return new MenuScene(_dungianoGame, buttons, new Background(_dungianoGame, "MenuBackgrounds/menuBackground"));
        }

        private void _createLevelButtons(List<Button> buttons, int page)
        {
            int start = page * 6 - 5;

            for (int i = start; i < start + 6; i++)
            {
                string iString = i.ToString();
                Vector2 position;

                switch (i % 6)
                {
                    case 1:
                        position = new Vector2(_screenSize.Width * 2 / 6, _screenSize.Height / 3);
                        break;
                    case 2:
                        position = new Vector2(_screenSize.Width * 3 / 6, _screenSize.Height / 3);
                        break;
                    case 3:
                        position = new Vector2(_screenSize.Width * 4 / 6, _screenSize.Height / 3);
                        break;
                    case 4:
                        position = new Vector2(_screenSize.Width * 2 / 6, _screenSize.Height * 2 / 3);
                        break;
                    case 5:
                        position = new Vector2(_screenSize.Width * 3 / 6, _screenSize.Height * 2 / 3);
                        break;
                    case 0:
                        position = new Vector2(_screenSize.Width * 4 / 6, _screenSize.Height * 2 / 3);
                        break;
                    default:
                        position = new Vector2(0, 0);
                        break;
                }

                buttons.Add(new LevelNavigationButton(
                    dungianoGame: _dungianoGame,
                    texturesNames: ("Buttons/Numbers/"+ iString +"/normal", "Buttons/Numbers/" + iString +"/lighter", "Buttons/Numbers/" + iString +"/darker", "Buttons/Numbers/" + iString +"/grey"),
                    position: position,
                    scale: 0.5f,
                    level: i
                    )
                );
            }
        }

        private void _setLevelButtonsDisabled(List<Button> buttons, int page)
        {
            int pageSize = 6;
            int maxLevel = _dungianoGame.GameData.MaxLevelVisited;

            int i = buttons.Count;
            int level = page * pageSize;

            while (i > buttons.Count - pageSize)
            {
                if (level > maxLevel)
                    buttons[i - 1].SetDisabled(true);

                i--;
                level--;
            }
        }

        public MenuScene CreateDieMenu(int previousLevel)
        {
            List<Button> buttons = new List<Button>();

            // playAgain button
            buttons.Add(new LevelNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/PlayAgain/normal", "Buttons/PlayAgain/lighter", "Buttons/PlayAgain/darker", "Buttons/PlayAgain/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 4),
                scale: 0.5f,
                level: previousLevel
                )
            );

            // level menu button
            buttons.Add(new MenuNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/MainMenu/normal", "Buttons/MainMenu/lighter", "Buttons/MainMenu/darker", "Buttons/MainMenu/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 2),
                scale: 0.5f,
                nextScene: SceneType.MainMenu
                )
            );

            //quit button
            buttons.Add(new ClassicButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/QuitLarge/normal", "Buttons/QuitLarge/lighter", "Buttons/QuitLarge/darker", "Buttons/QuitLarge/normal"),
                position: new Vector2(_screenSize.Width / 2, 3 * _screenSize.Height / 4),
                scale: 0.5f,
                function: _dungianoGame.QuitGame
                )
            );

            return new MenuScene(_dungianoGame, buttons, new Background(_dungianoGame, "MenuBackgrounds/dieMenuBackground"));
        }

        public MenuScene CreatePauseMenu(int previousLevel)
        {
            List<Button> buttons = new List<Button>();

            // continue button
            buttons.Add(new ClassicButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/Resume/normal", "Buttons/Resume/lighter", "Buttons/Resume/darker", "Buttons/Resume/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 4),
                scale: 0.5f,
                function: _dungianoGame.SceneManager.GoBack
                )
            );

            // level menu button
            buttons.Add(new LevelNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/PlayAgain/normal", "Buttons/PlayAgain/lighter", "Buttons/PlayAgain/darker", "Buttons/PlayAgain/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 2),
                scale: 0.5f,
                level: previousLevel
                )
            );

            //mainMenu button
            buttons.Add(new MenuNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/MainMenu/normal", "Buttons/MainMenu/lighter", "Buttons/MainMenu/darker", "Buttons/MainMenu/normal"),
                position: new Vector2(_screenSize.Width / 2, 3 * _screenSize.Height / 4),
                scale: 0.5f,
                nextScene: SceneType.MainMenu
                )
            );

            return new MenuScene(_dungianoGame, buttons, new Background(_dungianoGame, "MenuBackgrounds/pauseMenuBackground"));
        }

        public MenuScene CreateFinishMenu()
        {
            List<Button> buttons = new List<Button>();

            // level menu button
            buttons.Add(new MenuNavigationButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/MainMenu/normal", "Buttons/MainMenu/lighter", "Buttons/MainMenu/darker", "Buttons/MainMenu/normal"),
                position: new Vector2(_screenSize.Width / 2, _screenSize.Height / 2),
                scale: 0.5f,
                nextScene: SceneType.MainMenu
                )
            );

            //mainMenu button
            buttons.Add(new ClassicButton(
                dungianoGame: _dungianoGame,
                texturesNames: ("Buttons/QuitLarge/normal", "Buttons/QuitLarge/lighter", "Buttons/QuitLarge/darker", "Buttons/QuitLarge/normal"),
                position: new Vector2(_screenSize.Width / 2, 3 * _screenSize.Height / 4),
                scale: 0.5f,
                function: _dungianoGame.QuitGame
                )
            );

            return new MenuScene(_dungianoGame, buttons, new Background(_dungianoGame, "MenuBackgrounds/finishMenuBackground"));
        }


        public LevelScene CreateLevel(int level)
        {
            return new LevelScene(_dungianoGame, level);
        }

        public int NextPageMenu()
        {
            return _levelMenuPage + 1;
        }
    }
}

