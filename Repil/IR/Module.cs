﻿using System;
using System.Collections.Generic;
using System.Linq;
using Repil.Types;

namespace Repil.IR
{
    /// <summary>
    /// https://llvm.org/docs/LangRef.html
    /// clang -O3 -S -emit-llvm -fpic *.c
    /// </summary>
    public class Module
    {
        /// <summary>
        /// The original module identifier
        /// </summary>
        public string SourceFilename = "";

        /// <summary>
        /// How data is to be laid out in memory
        /// </summary>
        public string TargetDatalayout = "";

        /// <summary>
        /// A series of identifiers delimited by the minus sign character
        /// </summary>
        public string TargetTriple = "";

        public SymbolTable<StructureType> IdentifiedStructures = new SymbolTable<StructureType> ();

        public static Module Parse (string llvm)
        {
            var module = new Module ();
            var parser = new Parser (module);
            var lex = new Lexer (llvm);
            try {
                parser.yyparse (lex, null);
            }
            catch (Exception ex) {
                var m = $"{ex.Message}\n{lex.Surrounding}";
                throw new Exception (m, ex);
            }
            return module;
        }
    }

    public partial class Parser
    {
        Module module;

        public Parser (Module module)
        {
            this.module = module;
        }

        static List<T> NewList<T>(T firstItem)
        {
            return new List<T> (1) { firstItem };
        }
        static List<T> ListAdd<T> (object list, T item)
        {
            var l = (List<T>)list;
            l.Add (item);
            return l;
        }
    }
}
