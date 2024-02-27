﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MySourceGen
{
    internal static class GeneratorsUtils
    {
        private static readonly Version GeneratorVersion = new Version(1, 0, 0, 0);

        private static readonly string GeneratedCodeAttribute =
          $"[global::System.CodeDom.Compiler.GeneratedCode(\"{typeof(GeneratorsUtils).Assembly.GetName().Name}\", \"{GeneratorVersion}\")]";

        public static void WriteGeneratedFileHeader(StringBuilder sb)
        {
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("// This file was generated by a tool; you should avoid making direct changes.");
            sb.AppendLine("// Consider using the tool(s) that generated this file to make any changes.");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine();
        }

        public static void WriteGeneratedAttribute(StringBuilder sb)
        {
            sb.AppendLine(GeneratedCodeAttribute);
        }
    }
}