﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lc_cli.Library
{
    public class Loader
    {
        public static Dictionary LoadFromDir(string dirPath)
        {
            Dictionary output = new Dictionary();

            DirectoryInfo dir = new DirectoryInfo(dirPath);

            var files = dir.GetFiles("*.lcf");

            foreach(FileInfo fileInfo in files)
            {
                output.Add(fileInfo.Name.Split('.')[0], fileInfo.OpenText().ReadToEnd().Replace("\n", ""));
            }

            return output;
        }
    }
}
