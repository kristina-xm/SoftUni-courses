﻿using System;

namespace Box
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var numbers = int.Parse(Console.ReadLine());

            for (int i = 0; i < numbers; i++)
            {
                var input = int.Parse(Console.ReadLine());
                var box = new Box<int>(input);
                Console.WriteLine(box);
            }
        }
    }
}
