using System;
using System.IO;

namespace Brave_New_World
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isWork = true;
            int playerMaxHealth = 500;
            Console.Clear();
            bool alreadyPlayed = false;
            string selectedMenuItem;

            while (isWork)
            {
                Console.Write(GenerateStartMenu(alreadyPlayed));
                selectedMenuItem = Console.ReadLine();

                switch (selectedMenuItem)
                {
                    case "1":
                        LetsPlay(ref alreadyPlayed, ref playerMaxHealth);
                        break;
                    case "2":
                        Console.WriteLine("В Вайтране как раз появилась вакансия стражника");
                        Console.WriteLine("Нажмите любую клавишу для телепортации в Вайтран...");
                        Console.ReadKey();
                        isWork = false;
                        break;
                }
            }
        }

        static string GenerateStartMenu(bool alreadyPlayed)
        {
            string menuText;
            string menuLine1;
            string menuLine2 = "Готов ли ты отправиться в новое путешествие?\n";
            string menuLine3 = "1 - Конечно, ведь меня ведет дорога приключений!\n";
            string menuLine4 = "2 - Спасибо, мне уже прострелили колено.\n";

            if (alreadyPlayed)
            {
                menuLine1 = "Рад снова видеть тебя, герой!\n";
            }
            else
            {
                menuLine1 = "";
            }
            menuText = menuLine1 + menuLine2 + menuLine3 + menuLine4;
            return menuText;
        }

        static void LetsPlay(ref bool alreadyPlayed, ref int playerMaxHealth)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Random random = new Random();
            bool isPlaying = true;
            int playerHealth = playerMaxHealth;
            string mob1Name = "Болотная цапля";
            int mob1Difficulty = 50;
            bool mob1Alive = true;
            string mob2Name = "ШумелкаМышь";
            int mob2Difficulty = 40;
            bool mob2Alive = true;
            string mob3Name = "Жуткий волк";
            int mob3Difficulty = 60;
            bool mob3Alive = true;
            string mob4Name = "Древний леший";
            int mob4Difficulty = 80;
            bool mob4Alive = true;
            int playerPositionX;
            int playerPositionY;
            int mob1PositionX;
            int mob1PositionY;
            int mob2PositionX;
            int mob2PositionY;
            int mob3PositionX;
            int mob3PositionY;
            int playerDeltaX = 0;
            int playerDeltaY = 0;
            int mob1DeltaX = 1;
            int mob1DeltaY = 0;
            int mob2DeltaX = 1;
            int mob2DeltaY = 0;
            int mob3DeltaX = 0;
            int mob3DeltaY = -1;
            char[,] map = ReadMap("map1", out playerPositionX, out playerPositionY, out mob1PositionX, out mob1PositionY, out mob2PositionX, out mob2PositionY, out mob3PositionX, out mob3PositionY);
            DrawMap(map);
            int coordAdvenchureLog = map.GetLength(0) + 3;
            int coordHealthBar = map.GetLength(0) + 2;

            while (isPlaying)
            {
                Console.SetCursorPosition(0, coordHealthBar);
                Console.Write($"У вас {playerHealth} из {playerMaxHealth} здоровья");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDirection(key, ref playerDeltaX, ref playerDeltaY);
                }
                char nextCell = map[playerPositionX + playerDeltaX, playerPositionY + playerDeltaY];

                switch (nextCell)
                {
                    case '#':
                        playerDeltaX = 0;
                        playerDeltaY = 0;
                        break;
                    case '^':
                        HealPlayer(ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                        break;
                    case '?':
                        Fight(random, mob4Name, mob4Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog, ref mob4Alive);
                        break;
                    default:
                        break;
                }
                Move(map, '@', ref playerPositionX, ref playerPositionY, playerDeltaX, playerDeltaY);

                if (playerPositionX == mob1PositionX && playerPositionY == mob1PositionY)
                {
                    Fight(random, mob1Name, mob1Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog, ref mob1Alive);
                }

                if (playerPositionX == mob2PositionX && playerPositionY == mob2PositionY)
                {
                    Fight(random, mob2Name, mob2Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog, ref mob2Alive);
                }

                if (playerPositionX == mob3PositionX && playerPositionY == mob3PositionY)
                {
                    Fight(random, mob3Name, mob3Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog, ref mob3Alive);
                }
                MoveMob(random, '!', ',', map, ref mob1PositionX, ref mob1PositionY, ref mob1DeltaX, ref mob1DeltaY, ref mob1Alive);
                MoveMob(random, '*', ' ', map, ref mob2PositionX, ref mob2PositionY, ref mob2DeltaX, ref mob2DeltaY, ref mob2Alive);
                MoveMob(random, '$', '/', map, ref mob3PositionX, ref mob3PositionY, ref mob3DeltaX, ref mob3DeltaY, ref mob3Alive);
                System.Threading.Thread.Sleep(200);
            }
            alreadyPlayed = true;
            Console.Clear();
            Console.CursorVisible = true;
        }

        static char[,] ReadMap(string mapName, out int playerPositionX, out int playerPositionY, out int mob1PositionX, out int mob1PositionY, out int mob2PositionX, out int mob2PositionY, out int mob3PositionX, out int mob3PositionY)
        {
            playerPositionX = 0;
            playerPositionY = 0;
            mob1PositionX = 0;
            mob1PositionY = 0;
            mob2PositionX = 0;
            mob2PositionY = 0;
            mob3PositionX = 0;
            mob3PositionY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        playerPositionX = i;
                        playerPositionY = j;
                        map[i, j] = ' ';
                    }
                    else if (map[i, j] == '!')
                    {
                        mob1PositionX = i;
                        mob1PositionY = j;
                        map[i, j] = ',';
                    }
                    else if (map[i, j] == '*')
                    {
                        mob2PositionX = i;
                        mob2PositionY = j;
                        map[i, j] = ' ';
                    }
                    else if (map[i, j] == '$')
                    {
                        mob3PositionX = i;
                        mob3PositionY = j;
                        map[i, j] = '/';
                    }
                }
            }
            return map;
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Move(char[,] map, char symbol, ref int positionX, ref int positionY, int deltaX, int deltaY)
        {
            Console.SetCursorPosition(positionY, positionX);
            Console.Write(map[positionX, positionY]);
            positionX += deltaX;
            positionY += deltaY;
            Console.SetCursorPosition(positionY, positionX);
            Console.Write(symbol);
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int deltaX, ref int deltaY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    deltaX = -1;
                    deltaY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    deltaX = 1;
                    deltaY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    deltaX = 0;
                    deltaY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    deltaX = 0;
                    deltaY = 1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int deltaX, ref int deltaY)
        {
            int mobDirection = random.Next(1, 5);
            switch (mobDirection)
            {
                case 1:
                    deltaX = -1;
                    deltaY = 0;
                    break;
                case 2:
                    deltaX = 1;
                    deltaY = 0;
                    break;
                case 3:
                    deltaX = 0;
                    deltaY = -1;
                    break;
                case 4:
                    deltaX = 0;
                    deltaY = 1;
                    break;
            }
        }

        static void HealPlayer(ref int playerHealth, ref int playerMaxHealth, ref int logCoordinate)
        {
            if (playerHealth < playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
                Console.SetCursorPosition(0, logCoordinate);
                Console.Write("Вы нашли сад с молодильными яблоками\n");
            }
            else
            {
                Console.SetCursorPosition(0, logCoordinate);
                Console.Write("Вы нашли молодильные яблоки, но здоровья и так полно.\n");
            }
            logCoordinate++;
        }

        static void Fight(Random random, string mobName, int mobDifficulty, int coordHealthBar, ref bool isPlaying, ref int playerHealth, ref int playerMaxHealth, ref int coordAdvenchureLog, ref bool ismobAlive)
        {
            bool isAlive = true;
            int percent = 100;
            int playerDamageModifier = 10;
            int maxDamageModifier = 2;
            int playerMinDamage = playerMaxHealth * playerDamageModifier / percent;
            int playerMaxDamage = playerMinDamage * maxDamageModifier;
            int mobHealth = playerMaxHealth * mobDifficulty / percent;
            int mobDamageModifier = 25;
            int mobMinDamage = mobHealth * mobDamageModifier / percent;
            int mobMaxDamage = mobMinDamage * maxDamageModifier;
            Console.SetCursorPosition(0, coordAdvenchureLog);
            Console.Write("Вы встретили " + mobName + "!\n");
            coordAdvenchureLog++;

            while (isAlive)
            {
                mobHealth -= random.Next(playerMinDamage, playerMaxDamage);
                playerHealth -= random.Next(mobMinDamage, mobMaxDamage);

                if (playerHealth <= 0 || mobHealth <= 0)
                {
                    isAlive = false;
                }
            }
            ismobAlive = CheckFightResult(mobName, mobHealth, ref playerMaxHealth, ref playerHealth, coordHealthBar, ref coordAdvenchureLog, ref isPlaying);
        }

        static void MoveMob(Random random, char mobSymbol, char mobAreaSymbol, char[,] map, ref int mobPositionX, ref int mobPositionY, ref int mobDeltaX, ref int mobDeltaY, ref bool isMobAlive)
        {
            if (isMobAlive)
            {
                if (map[mobPositionX + mobDeltaX, mobPositionY + mobDeltaY] == mobAreaSymbol)
                {
                    Move(map, mobSymbol, ref mobPositionX, ref mobPositionY, mobDeltaX, mobDeltaY);
                }
                else
                {
                    ChangeDirection(random, ref mobDeltaX, ref mobDeltaY);
                }
            }
            else
            {
                mobPositionX = 0;
                mobPositionY = 0;
            }

        }
        static bool CheckFightResult(string mobName, int mobHealth, ref int playerMaxHealth, ref int playerHealth, int coordHealthBar, ref int coordAdvenchureLog, ref bool isPlaying)
        {
            int bonusHealth = 50;
            bool ismobAlive;

            if (mobHealth <= 0)
            {
                playerMaxHealth += bonusHealth;
                Console.SetCursorPosition(0, coordHealthBar);
                Console.Write($"У вас {playerHealth} из {playerMaxHealth} здоровья.     ");
                Console.SetCursorPosition(0, coordAdvenchureLog);
                Console.Write("Вы победили " + mobName + " и получаете в награду +" + bonusHealth + " единиц здоровья\n");
                coordAdvenchureLog++;
                ismobAlive = false;
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Не повезло! {mobName} оказался сильнее! Вы теряете {bonusHealth} здоровья.\nПопробуйте еще раз.");
                playerMaxHealth -= bonusHealth;
                isPlaying = false;
                ismobAlive = true;
                Console.ReadKey();
            }
            return ismobAlive;
        }
    }
}