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
            int mob3Difficulty = 70;
            bool mob3Alive = true;
            string mob4Name = "Древний леший";
            int mob4Difficulty = 90;
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
                        isPlaying = Fight(random, mob4Name, mob4Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                        break;
                    default:
                        break;
                }
                Move(map, '@', ref playerPositionX, ref playerPositionY, playerDeltaX, playerDeltaY);

                if (playerPositionX == mob1PositionX && playerPositionY == mob1PositionY)
                {
                    mob1Alive = Fight(random, mob1Name, mob1Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (playerPositionX == mob2PositionX && playerPositionY == mob2PositionY)
                {
                    mob2Alive = Fight(random, mob2Name, mob2Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (playerPositionX == mob3PositionX && playerPositionY == mob3PositionY)
                {
                    mob3Alive = Fight(random, mob3Name, mob3Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (mob1Alive)
                {
                    if (map[mob1PositionX + mob1DeltaX, mob1PositionY + mob1DeltaY] == ',')
                    {
                        Move(map, '!', ref mob1PositionX, ref mob1PositionY, mob1DeltaX, mob1DeltaY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob1DeltaX, ref mob1DeltaY);
                    }
                }
                else
                {
                    mob1PositionX = 0;
                    mob1PositionY = 0;
                }

                if (mob2Alive)
                {
                    if (map[mob2PositionX + mob2DeltaX, mob2PositionY + mob2DeltaY] == ' ')
                    {
                        Move(map, '*', ref mob2PositionX, ref mob2PositionY, mob2DeltaX, mob2DeltaY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob2DeltaX, ref mob2DeltaY);
                    }
                }
                else
                {
                    mob2PositionX = 0;
                    mob2PositionY = 0;
                }

                if (mob3Alive)
                {
                    if (map[mob3PositionX + mob3DeltaX, mob3PositionY + mob3DeltaY] == '/')
                    {
                        Move(map, '$', ref mob3PositionX, ref mob3PositionY, mob3DeltaX, mob3DeltaY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob3DeltaX, ref mob3DeltaY);
                    }
                }
                else
                {
                    mob3PositionX= 0;
                    mob3PositionY = 0;
                }
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

        static bool Fight(Random random, string mobName, int mobDifficulty, int coordHealthBar, ref bool isPlaying, ref int playerHealth, ref int playerMaxHealth, ref int coordAdvenchureLog)
        {
            bool isAlive = true;
            bool ismobAlive = true;
            int percent = 100;
            int playerDamageModifier = 10;
            int playerMinDamage = playerMaxHealth * playerDamageModifier / percent;
            int playerMaxDamage = playerMinDamage * 2;
            int mobHealth = playerMaxHealth * mobDifficulty / percent;
            int mobDamageModifier = 25;
            int mobMinDamage = mobHealth * mobDamageModifier / percent;
            int mobMaxDamage = mobMinDamage * 2;
            Console.SetCursorPosition(0, coordAdvenchureLog);
            Console.Write("Вы встретили " + mobName + "!\n");
            coordAdvenchureLog++;
            int bonusHealth = 50;

            while (isAlive)
            {
                mobHealth -= random.Next(playerMinDamage, playerMaxDamage);
                playerHealth -= random.Next(mobMinDamage, mobMaxDamage);

                if (playerHealth <= 0 || mobHealth <= 0)
                {
                    isAlive = false;
                }
            }
            if (mobHealth <= 0)
            {
                playerMaxHealth += bonusHealth;
                Console.SetCursorPosition(0, coordHealthBar);
                Console.Write($"У вас {playerHealth} из {playerMaxHealth} здоровья.     ");
                Console.SetCursorPosition(0, coordAdvenchureLog);
                Console.Write("Вы победили " + mobName + " и получаете в награду +" + bonusHealth + " единиц здоровья\n");
                coordAdvenchureLog++;
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Не повезло! {mobName} оказался сильнее! Вы теряете {bonusHealth} здоровья.\nПопробуйте еще раз.");
                playerMaxHealth -= bonusHealth;
                isPlaying = false;
                Console.ReadKey();
            }
            ismobAlive = false;
            return ismobAlive;
        }
    }
}