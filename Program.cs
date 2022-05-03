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
            int playerX;
            int playerY;
            int mob1X;
            int mob1Y;
            int mob2X;
            int mob2Y;
            int mob3X;
            int mob3Y;
            int playerDX = 0;
            int playerDY = 0;
            int mob1DX = 1;
            int mob1DY = 0;
            int mob2DX = 1;
            int mob2DY = 0;
            int mob3DX = 0;
            int mob3DY = -1;
            char[,] map = ReadMap("map1", out playerX, out playerY, out mob1X, out mob1Y, out mob2X, out mob2Y, out mob3X, out mob3Y);
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
                    ChangeDirection(key, ref playerDX, ref playerDY);
                }
                char nextCell = map[playerX + playerDX, playerY + playerDY];

                switch (nextCell)
                {
                    case '#':
                        playerDX = 0;
                        playerDY = 0;
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
                Move(map, '@', ref playerX, ref playerY, playerDX, playerDY);

                if (playerX == mob1X && playerY == mob1Y)
                {
                    mob1Alive = Fight(random, mob1Name, mob1Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (playerX == mob2X && playerY == mob2Y)
                {
                    mob2Alive = Fight(random, mob2Name, mob2Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (playerX == mob3X && playerY == mob3Y)
                {
                    mob3Alive = Fight(random, mob3Name, mob3Difficulty, coordHealthBar, ref isPlaying, ref playerHealth, ref playerMaxHealth, ref coordAdvenchureLog);
                }

                if (mob1Alive)
                {
                    if (map[mob1X + mob1DX, mob1Y + mob1DY] == ',')
                    {
                        Move(map, '!', ref mob1X, ref mob1Y, mob1DX, mob1DY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob1DX, ref mob1DY);
                    }
                }
                else
                {
                    mob1X = 0;
                    mob1Y = 0;
                }

                if (mob2Alive)
                {
                    if (map[mob2X + mob2DX, mob2Y + mob2DY] == ' ')
                    {
                        Move(map, '*', ref mob2X, ref mob2Y, mob2DX, mob2DY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob2DX, ref mob2DY);
                    }
                }
                else
                {
                    mob2X = 0;
                    mob2Y = 0;
                }

                if (mob3Alive)
                {
                    if (map[mob3X + mob3DX, mob3Y + mob3DY] == '/')
                    {
                        Move(map, '$', ref mob3X, ref mob3Y, mob3DX, mob3DY);
                    }
                    else
                    {
                        ChangeDirection(random, ref mob3DX, ref mob3DY);
                    }
                }
                else
                {
                    mob3X= 0;
                    mob3Y = 0;
                }
                System.Threading.Thread.Sleep(200);
            }
            alreadyPlayed = true;
            Console.Clear();
            Console.CursorVisible = true;
        }

        static char[,] ReadMap(string mapName, out int playerX, out int playerY, out int mob1X, out int mob1Y, out int mob2X, out int mob2Y, out int mob3X, out int mob3Y)
        {
            playerX = 0;
            playerY = 0;
            mob1X = 0;
            mob1Y = 0;
            mob2X = 0;
            mob2Y = 0;
            mob3X = 0;
            mob3Y = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        playerX = i;
                        playerY = j;
                        map[i, j] = ' ';
                    }
                    else if (map[i, j] == '!')
                    {
                        mob1X = i;
                        mob1Y = j;
                        map[i, j] = ',';
                    }
                    else if (map[i, j] == '*')
                    {
                        mob2X = i;
                        mob2Y = j;
                        map[i, j] = ' ';
                    }
                    else if (map[i, j] == '$')
                    {
                        mob3X = i;
                        mob3Y = j;
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

        static void Move(char[,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X, Y]);
            X += DX;
            Y += DY;
            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1;
                    DY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1;
                    DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0;
                    DY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0;
                    DY = 1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int mobDirection = random.Next(1, 5);
            switch (mobDirection)
            {
                case 1:
                    DX = -1;
                    DY = 0;
                    break;
                case 2:
                    DX = 1;
                    DY = 0;
                    break;
                case 3:
                    DX = 0;
                    DY = -1;
                    break;
                case 4:
                    DX = 0;
                    DY = 1;
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
                playerMaxHealth += 50;
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
